using Axis.Luna;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.RBAC.Auth;
using Axis.Pollux.RBAC.Services;

using System.Collections.Generic;

namespace Axis.Pollux.RoleAuthorization.Services
{
    using PermissionMap = Dictionary<string, IEnumerable<Permission>>;

    public class CachableRoleAuthority: RoleAuthority
    {
        private WeakCache _cache = new WeakCache();


        public CachableRoleAuthority(Jupiter.IDataContext dataContext)
        : base(dataContext)
        {
        }

        public override PermissionMap UserPermissions(User user)
            => _cache.GetOrAdd(user.UserId, _uid => base.UserPermissions(user));

        public override Operation AssignRole(User user, string role)
            => base.AssignRole(user, role)
                   .UsingValue(_v => _cache.Invalidate(user.UserId));

        public override Operation DeleteUserRole(User user, Role role)
            => base.DeleteUserRole(user, role)
                   .UsingValue(_v => _cache.Invalidate(user.UserId));

        public override Operation DeleteRole(Role role)
            => base.DeleteRole(role)
                   .UsingValue(_v => _cache.InvalidateAll());
    }
}
