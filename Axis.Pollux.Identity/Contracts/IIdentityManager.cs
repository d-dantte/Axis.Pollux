using Axis.Luna.Operation;
using Axis.Pollux.Identity.Models;
using System;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IIdentityManager
    {
        Operation<UserIdentity> AssignIdentity(Guid userId, string userName);
        Operation<UserIdentity> ChangeIdentity(Guid userId, string newUserName);
    }
}
