using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.RBAC.Auth;
using Axis.Pollux.RBAC.Services;

using System.Collections.Generic;

namespace Axis.Pollux.RoleAuthorization.Services
{
    public class CachableRoleAuthority: RoleAuthority
    {
        private WeakCache _cache = new WeakCache();


        public CachableRoleAuthority(Jupiter.IDataContext dataContext)
        : base(dataContext)
        {
        }

        protected override Dictionary<string, IEnumerable<Permission>> GetRolePermissions(User user)
            => _cache.GetOrAdd(user.UserId, _uid => base.GetRolePermissions(user));
    }
}
