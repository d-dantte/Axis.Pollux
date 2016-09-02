using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using System;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.RBAC.Auth;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Services
{
    public class RoleAuthority : IUserAuthorization
    {
        private Jupiter.IDataContext _context = null;

        public RoleAuthority(Jupiter.IDataContext dataContext)
        {
            ThrowNullArguments(() => dataContext);

            _context = dataContext;
        }

        #region IUSerAuthentication
        public Operation AssignRole(User user, string role)
            => Operation.Run(() =>
            {
                var userRoleStore = _context.Store<UserRole>();
                var roleStore = _context.Store<Role>();
                if(roleStore.Query.Any(r => r.RoleName == role) &&
                   !userRoleStore.Query.Any(ur => ur.RoleName == role && ur.UserId == user.EntityId))
                {
                    var userRole = userRoleStore.NewObject();
                    userRole.RoleName = role;
                    userRole.UserId = user.UserId;
                    userRoleStore.Add(userRole);
                    _context.CommitChanges();
                }
            });

        public IQueryable<Role> UserRoles(User user)
            => from ur in _context.Store<UserRole>().Query
               join r in _context.Store<Role>().Query on ur.RoleName equals r.EntityId
               where ur.UserId == user.EntityId
               select r;

        public Operation Authorize(PermissionProfile authRequest)
            => Operation.Run(() =>
            {
                var roles = UserRoles(authRequest.Principal).AsEnumerable();
                if (roles.Any(r => authRequest.Deny.Contains(r.Value.ToString()))) throw new Exception("Access Denied");
                else if (!roles.Any(r => authRequest.Grant.Contains(r.Value.ToString()))) throw new Exception("Access Denied");
                //else return
            });

        public Operation<Role> CreateRole(string name)
            => Operation.Run(() => _context.Store<Role>()
                                           .NewObject()
                                           .With(new { RoleName = name }));

        public Operation DeleteUserRole(User user, Role role)
            => Operation.Run(() =>
            {
                var urstore = _context.Store<UserRole>();
                var ur = urstore.Query.FirstOrDefault(_ur => _ur.UserId == user.EntityId && _ur.RoleName == role.RoleName);
                if (ur != null) urstore.Delete(ur, true);
                else throw new Exception("Failed");
            });

        public Operation AddRole(string role)
            => Operation.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(role)) throw new Exception("Invalid Role Name specified");
                var store = _context.Store<Role>();
                if(!store.Query.Any(r => r.RoleName == role))
                {
                    store.Add(new Role { RoleName = role });
                    _context.CommitChanges();
                }
            });

        public Operation DeleteRole(Role role)
            => Operation.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(role?.RoleName)) throw new Exception("Invalid Role Name specified");
                _context.Store<Role>().Delete(role, true);
            });
        #endregion
    }
}
