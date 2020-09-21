using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;
using System;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.Services.Queries
{
    public interface IIdentityQueries
    {
        Task<User> GetUserById(Guid userId);
        Task<UserIdentity> GetIdentityById(Guid identityId);
        Task<UserIdentity> GetIdentityByUserId(Guid userId);
        Task<ArrayPage<UserIdentity>> GetAllIdentities(ArrayPageRequest request = null);
    }
}
