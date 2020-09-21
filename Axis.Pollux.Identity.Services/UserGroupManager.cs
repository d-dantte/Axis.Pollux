using static Axis.Luna.Extensions.ExceptionExtension;

using Axis.Jupiter;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Exceptions;

namespace Axis.Pollux.Identity.Services
{
    public class UserGroupManager : IUserGroupManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserGroupQueries _groupQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;

        public UserGroupManager(IUserQueries groupQueries, IDataAccessAuthorizer dataAuthorizer, StoreProvider storeProvider)
        {
            ThrowNullArguments(
                nameof(groupQueries).ObjectPair(groupQueries),
                nameof(dataAuthorizer).ObjectPair(dataAuthorizer),
                nameof(storeProvider).ObjectPair(storeProvider));

            _groupQueries = groupQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }

        public Operation<UserGroup> AddMember(Guid groupId, Guid memberId)
        => Operation.Try(async () =>
        {
            if (groupId == default(Guid)
                || memberId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(UserGroup).FullName,
                intent: Authorization.Models.DataAccessIntent.Write));

            var member = (await _groupQueries
                .GetUserById(memberId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(UserGroup).FullName);

            return (await storeCommand
                .Add(nameData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<UserGroup> CreateGroup(string name, int status)
        => Operation.Try(async () =>
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var group = new UserGroup
            {
                Id = Guid.NewGuid(),
                Status = status,
                Name = name
            };
            var storeCommand = _storeProvider.CommandFor(typeof(UserGroup).FullName);

            return (await storeCommand
                .Add(group))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<UserGroup> DeleteGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }

        public Operation RemoveMember(Guid gruopId, User member)
        {
            throw new NotImplementedException();
        }

        public Operation UpdateGroupStatus(Guid groupId, int status)
        {
            throw new NotImplementedException();
        }
    }
}
