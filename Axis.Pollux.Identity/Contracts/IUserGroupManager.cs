using Axis.Luna.Operation;
using Axis.Pollux.Identity.Models;
using System;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IUserGroupManager
    {
        Operation<UserGroup> CreateGroup(string name, int status = 0);

        Operation<UserGroup> AddMember(Guid groupId, Guid memberId);

        Operation RemoveMember(Guid gruopId, Guid memberId);

        Operation UpdateGroupStatus(Guid groupId, int status);

        Operation<UserGroup> DeleteGroup(Guid groupId);
    }
}
