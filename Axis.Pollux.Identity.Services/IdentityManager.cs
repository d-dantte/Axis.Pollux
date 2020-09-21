using static Axis.Luna.Extensions.ExceptionExtension;

using Axis.Jupiter;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.Queries;
using System;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Exceptions;

namespace Axis.Pollux.Identity.Services
{
    public class IdentityManager : IIdentityManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IIdentityQueries _identityQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;
        private readonly IUserNameValidator _userNameValidator;


        public IdentityManager(
            IIdentityQueries identityQueries,
            IUserNameValidator userNameValidator,
            IDataAccessAuthorizer dataAuthorizer,
            StoreProvider storeProvider)
        {
            ThrowNullArguments(
                nameof(identityQueries).ObjectPair(identityQueries),
                nameof(dataAuthorizer).ObjectPair(dataAuthorizer),
                nameof(userNameValidator).ObjectPair(userNameValidator),
                nameof(storeProvider).ObjectPair(storeProvider));

            _identityQueries = identityQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
            _userNameValidator = userNameValidator;
        }


        public Operation<UserIdentity> AssignIdentity(Guid userId, string userName)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var user = await _identityQueries
                .GetUserById(userId)
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument));

            var identity = await _identityQueries
                .GetIdentityByUserId(userId)
                .ThrowIfNotNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            identity = new UserIdentity
            {
                Id = Guid.NewGuid(),
                Owner = user,
                Name = await _userNameValidator.ValidateUsername(userName)
            };

            var storeCommand = _storeProvider.CommandFor(typeof(UserIdentity).FullName);

            return (await storeCommand
                .Add(identity))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<UserIdentity> ChangeIdentity(Guid userId, string newUserName)
        => Operation.Try(async () =>
        {
            if(userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            if(string.IsNullOrWhiteSpace(newUserName))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var identity = await _identityQueries
                .GetIdentityByUserId(userId)
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(UserIdentity).FullName,
                dataId: identity.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

            if (identity.Name.Equals(newUserName, StringComparison.InvariantCulture))
                throw new IdentityException(Common.Exceptions.ErrorCodes.GeneralError);

            identity.Name = await _userNameValidator.ValidateUsername(newUserName);
            var storeCommand = _storeProvider.CommandFor(typeof(UserIdentity).FullName);

            return (await storeCommand
                .Update(identity))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });
    }
}
