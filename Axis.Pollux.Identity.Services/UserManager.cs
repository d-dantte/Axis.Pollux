using System;
using Axis.Jupiter;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Exceptions;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.AccessDescriptors;
using Axis.Pollux.Identity.Services.Queries;

using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Identity.Services
{
    public class UserManager: IUserManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserQueries _userQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public UserManager(IUserQueries userQueries, IDataAccessAuthorizer dataAuthorizer, StoreProvider storeProvider)
        {
            ThrowNullArguments(
                nameof(userQueries).ObjectPair(userQueries),
                nameof(dataAuthorizer).ObjectPair(dataAuthorizer),
                nameof(storeProvider).ObjectPair(storeProvider));

            _userQueries = userQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }

        #region User

        public Operation<User> CreateUser(int? status = null)
        => Operation.Try(async () =>
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Status =  status ?? 0
            };
            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);

            return (await storeCommand
                .Add(user))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<User> DeleteUser(Guid userId)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(User).FullName,
                OwnerId = userId,
                DataId = userId.ToString()
            });

            var user = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);

            return (await storeCommand
                .Delete(user))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });
        
        public Operation UpdateUserStatus(Guid userId, int status)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(User).FullName,
                OwnerId = userId,
                DataId = userId.ToString()
            });

            var user = (await _userQueries
                    .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            user.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);

            (await storeCommand
                .Update(user))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<User> GetUser(Guid userId)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new UserOwnedData
            {
                DataType = typeof(User).FullName,
                OwnerId = userId,
                DataId = userId.ToString()
            });

            return (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));
        });

        public Operation<long> UserCount()
        => Operation.Try(async () =>
        {
            var count = await _userQueries.UserCount();
            return count.ThrowIf(c => c < 0, new IdentityException(ErrorCodes.InvalidStoreQueryResult));
        });

        #endregion

    }
}
