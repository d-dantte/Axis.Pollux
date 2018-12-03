using System;
using System.Threading.Tasks;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Identity.Contracts
{
    public interface IUserManager
    {
        #region User

        Operation<User> CreateUser(int? status = null);

        Operation<User> DeleteUser(Guid userId);

        Operation UpdateUserStatus(Guid userId, int status);

        Operation<User> GetUser(Guid userId);
        Operation<UserProfile> GetUserProfile(Guid userId);
        Operation<long> UserCount();

        #endregion
    }
}
