using System;
using System.Collections.Generic;
using System.Linq;
using Axis.Jupiter;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Exceptions;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.Queries;

using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Identity.Services
{
    public class ContactDataManager: IContactDataManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserQueries _userQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public ContactDataManager(IUserQueries userQueries, IDataAccessAuthorizer dataAuthorizer, StoreProvider storeProvider)
        {
            ThrowNullArguments(
                nameof(userQueries).ObjectPair(userQueries),
                nameof(dataAuthorizer).ObjectPair(dataAuthorizer),
                nameof(storeProvider).ObjectPair(storeProvider));

            _userQueries = userQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }

        #region ContactData

        public Operation<ContactData> AddContactData(Guid userId, ContactData contactData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            await contactData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                intent: Authorization.Models.DataAccessIntent.Write));

            contactData.Id = Guid.NewGuid();
            contactData.IsVerified = false;
            contactData.IsPrimary = false;
            contactData.Owner = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            return (await storeCommand
                .Add(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> DeleteContactData(Guid contactDataId)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(AddressData).FullName,
                dataId: contactData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Delete));

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            return (await storeCommand
                .Delete(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> UpdateContactData(ContactData contactData)
        => Operation.Try(async () =>
        {
            await contactData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            var persisted = (await _userQueries
                .GetContactDataById(contactData.Id))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                dataId: persisted.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            //copy values
            persisted.Channel = contactData.Channel;
            persisted.Data = contactData.Data;
            persisted.IsPrimary = contactData.IsPrimary;

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);
            return (await storeCommand
                .Update(persisted))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation UpdateContactDataStatus(Guid contactDataId, int status)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                    .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                dataId: contactData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            contactData.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation VerifyContactData(Guid contactDataId)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                    .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                dataId: contactData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            contactData.IsVerified = true;
            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> MakePrimary(Guid contactDataId, bool isPrimary)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                dataId: contactDataId.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            if (contactData.IsPrimary)
                return contactData;

            //find and reset the current primary contact
            var currentPrimary = await _userQueries
                .GetPrimaryUserContactData(contactData.Owner.Id, contactData.Channel)
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //get the store command
            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            //reset the current primary contact
            if (currentPrimary != null)
            {
                currentPrimary.IsPrimary = false;
                (await storeCommand
                    .Update(currentPrimary))
                    .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
            }

            //set the primary flag
            contactData.IsPrimary = true;
            return (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> AddTags(Guid contactDataId, params string[] tags)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                dataId: contactData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            //add the tags, using a hash set to get rid of duplicates
            contactData.Tags = new HashSet<string>(contactData.Tags)
                .AddRange(tags)
                .ToArray();

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);
            return (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> RemoveTags(Guid contactDataId, params string[] tags)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                dataId: contactData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            //remove the tags
            var tagBuffer = new HashSet<string>(contactData.Tags);
            tagBuffer.RemoveAll(tags);
            contactData.Tags = tagBuffer.ToArray();

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);
            return (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> GetContactData(Guid contactDataId)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                dataId: contactData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Read));

            return contactData;
        });

        public Operation<ArrayPage<ContactData>> GetUserContacts(Guid userId, ArrayPageRequest request = null)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                intent: Authorization.Models.DataAccessIntent.Read));

            return (await _userQueries
                .GetUserContactData(userId, request))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        public Operation<ContactData> GetPrimaryUserContact(Guid userId, string channel)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid)
                || string.IsNullOrWhiteSpace(channel))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                intent: Authorization.Models.DataAccessIntent.Read));

            return (await _userQueries
                .GetPrimaryUserContactData(userId, channel))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        public Operation<ArrayPage<ContactData>> GetUserContacts(
            Guid userId, 
            string[] communicationChannels, 
            string[] tags, 
            ArrayPageRequest arrayPageRequest)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(ContactData).FullName,
                intent: Authorization.Models.DataAccessIntent.Read));

            return (await _userQueries
                .GetUserContactData(userId, communicationChannels, tags, arrayPageRequest))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        #endregion
    }
}
