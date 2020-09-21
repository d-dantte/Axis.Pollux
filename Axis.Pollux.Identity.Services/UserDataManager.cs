using System;
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
    public class UserDataManager: IUserDataManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserQueries _userQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public UserDataManager(IUserQueries userQueries, IDataAccessAuthorizer dataAuthorizer, StoreProvider storeProvider)
        {
            ThrowNullArguments(
                nameof(userQueries).ObjectPair(userQueries),
                nameof(dataAuthorizer).ObjectPair(dataAuthorizer),
                nameof(storeProvider).ObjectPair(storeProvider));

            _userQueries = userQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }

        #region UserData

        public Operation<UserData> AddUserData(Guid userId, UserData userData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            await userData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(NameData).FullName,
                intent: Authorization.Models.DataAccessIntent.Write));

            userData.Id = Guid.NewGuid();
            userData.Owner = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(NameData).FullName);

            return (await storeCommand
                .Add(userData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<UserData> DeleteUserData(Guid userDataId)
        => Operation.Try(async () =>
        {
            if (userDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var userData = (await _userQueries
                .GetUserDataById(userDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(UserData).FullName,
                dataId: userData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Delete));

            var storeCommand = _storeProvider.CommandFor(typeof(UserData).FullName);

            return (await storeCommand
                .Delete(userData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<UserData> UpdateUserData(UserData userData)
        => Operation.Try(async () =>
        {
            await userData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            var persisted = (await _userQueries
                .GetUserDataById(userData.Id))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(UserData).FullName,
                dataId: persisted.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            //copy values
            persisted.Data = userData.Data;
            persisted.Name = userData.Data;
            persisted.Type = userData.Type;

            var storeCommand = _storeProvider.CommandFor(typeof(UserData).FullName);
            return (await storeCommand
                .Update(persisted))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation UpdateUserDataStatus(Guid userDataId, int status)
        => Operation.Try(async () =>
        {
            if (userDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var userData = (await _userQueries
                .GetContactDataById(userDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(UserData).FullName,
                dataId: userData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            userData.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(UserData).FullName);

            (await storeCommand
                .Update(userData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<UserData> GetUserData(Guid userDataId)
        => Operation.Try(async () =>
        {
            if (userDataId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var userData = (await _userQueries
                .GetUserDataById(userDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(UserData).FullName,
                dataId: userData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Read));

            return userData;
        });

        public Operation<ArrayPage<UserData>> GetUserUser(Guid userId, ArrayPageRequest request = null)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(UserData).FullName,
                intent: Authorization.Models.DataAccessIntent.Read));

            return (await _userQueries
                .GetUserData(userId, request))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        #endregion
    }
}
