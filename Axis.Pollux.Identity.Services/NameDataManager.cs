using System;
using Axis.Jupiter;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Exceptions;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.AccessDescriptors;
using Axis.Pollux.Identity.Services.Queries;

using static Axis.Luna.Extensions.ExceptionExtension;


namespace Axis.Pollux.Identity.Services
{
    public class NameDataManager: INameDataManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserQueries _userQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public NameDataManager(IUserQueries userQueries, IDataAccessAuthorizer dataAuthorizer, StoreProvider storeProvider)
        {
            ThrowNullArguments(
                nameof(userQueries).ObjectPair(userQueries),
                nameof(dataAuthorizer).ObjectPair(dataAuthorizer),
                nameof(storeProvider).ObjectPair(storeProvider));

            _userQueries = userQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }


        #region NameData

        public Operation<NameData> AddNameData(Guid userId, NameData nameData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            await nameData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(NameData).FullName,
                OwnerId = userId
            });

            nameData.Id = Guid.NewGuid();
            nameData.Owner = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(NameData).FullName);

            return (await storeCommand
                .Add(nameData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<NameData> DeleteNameData(Guid nameDataId)
        => Operation.Try(async () =>
        {
            if (nameDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var nameData = (await _userQueries
                .GetNameDataById(nameDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(NameData).FullName,
                OwnerId = nameData.Owner.Id,
                DataId = nameData.Id.ToString()
            });

            var storeCommand = _storeProvider.CommandFor(typeof(NameData).FullName);

            return (await storeCommand
                .Delete(nameData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<NameData> UpdateNameData(NameData nameData)
        => Operation.Try(async () =>
        {
            await nameData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            var persisted = (await _userQueries
                .GetNameDataById(nameData.Id))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(NameData).FullName,
                OwnerId = persisted.Owner.Id,
                DataId = persisted.Id.ToString()
            });

            //copy values
            persisted.FirstName = nameData.FirstName;
            persisted.LastName = nameData.LastName;
            persisted.MiddleName = nameData.MiddleName;

            var storeCommand = _storeProvider.CommandFor(typeof(NameData).FullName);
            return (await storeCommand
                .Update(persisted))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation UpdateNameDataStatus(Guid nameDataId, int status)
        => Operation.Try(async () =>
        {
            if (nameDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                    .GetContactDataById(nameDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(ContactData).FullName,
                OwnerId = contactData.Owner.Id,
                DataId = contactData.Id.ToString()
            });

            contactData.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<NameData> GetNameData(Guid nameDataId)
        => Operation.Try(async () =>
        {
            if (nameDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var nameData = (await _userQueries
                .GetNameDataById(nameDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(NameData).FullName,
                OwnerId = nameData.Owner.Id,
                DataId = nameData.Id.ToString()
            });

            return nameData;
        });

        public Operation<ArrayPage<NameData>> GetUserName(Guid userId, ArrayPageRequest request = null)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(NameData).FullName,
                OwnerId = userId
            });

            return (await _userQueries
                .GetUserNameData(userId, request))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        #endregion
    }
}
