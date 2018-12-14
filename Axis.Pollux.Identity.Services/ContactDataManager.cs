using System;
using Axis.Jupiter;
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
            ThrowNullArguments(() => userQueries,
                () => dataAuthorizer,
                () => storeProvider);

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
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(ContactData).FullName, userId);

            contactData.Id = Guid.NewGuid();
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
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(AddressData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

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
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                persisted.Owner.Id,
                persisted.Id.ToString());

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
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

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
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

            contactData.IsVerified = true;
            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            (await storeCommand
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
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

            return contactData;
        });

        public Operation<ArrayPage<ContactData>> GetUserContact(Guid userId, ArrayPageRequest request = null)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                userId);

            return (await _userQueries
                .GetUserContactData(userId, request))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        #endregion
    }
}
