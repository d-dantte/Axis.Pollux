using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using System;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.RBAC.Auth;
using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;

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
        public virtual Operation AssignRole(User user, string role)
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

        public virtual IQueryable<Role> UserRoles(User user)
            => from ur in _context.Store<UserRole>().Query
               join r in _context.Store<Role>().Query on ur.RoleName equals r.EntityId
               where ur.UserId == user.EntityId
               select r;

        public virtual Operation<Role> CreateRole(string name)
            => Operation.Run(() => _context.Store<Role>()
                                           .NewObject()
                                           .With(new { RoleName = name }));

        public virtual Operation DeleteUserRole(User user, Role role)
            => Operation.Run(() =>
            {
                var urstore = _context.Store<UserRole>();
                var ur = urstore.Query.FirstOrDefault(_ur => _ur.UserId == user.EntityId && _ur.RoleName == role.RoleName);
                if (ur != null) urstore.Delete(ur, true);
                else throw new Exception("Failed");
            });

        public virtual Operation AddRole(string role)
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

        public virtual Operation DeleteRole(Role role)
            => Operation.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(role?.RoleName)) throw new Exception("Invalid Role Name specified");
                _context.Store<Role>().Delete(role, true);
            });



        public virtual Operation AuthorizeAccess(PermissionProfile authRequest, Action operation = null)
            => Operation.Try(() =>
            {
                ValidatePermissionProfile(authRequest);
                operation?.Invoke();
            });

        public virtual Operation<T> AuthorizeAccess<T>(PermissionProfile authRequest, Func<T> operation = null)
            => Operation.Try(() =>
            {
                ValidatePermissionProfile(authRequest);
                if (operation != null) return operation.Invoke();
                else return default(T);
            });

        public virtual Operation AuthorizeAccess(PermissionProfile authRequest, Func<Operation> operation = null)
            => Operation.Try(() =>
            {
                ValidatePermissionProfile(authRequest);
                return operation?.Invoke() ?? Operation.NoOp();
            });

        public virtual Operation<T> AuthorizeAccess<T>(PermissionProfile authRequest, Func<Operation<T>> operation = null)
            => Operation.Try(() =>
            {
                ValidatePermissionProfile(authRequest);
                return operation?.Invoke() ?? Operation.NoOp<T>();
            });

        private int abcd() => 0;
        #endregion

        /// <summary>
        /// Validates the permission profile by checking that the principal has the right to access all the given resources
        /// </summary>
        /// <param name="authRequest"></param>
        protected void ValidatePermissionProfile(PermissionProfile authRequest)
        {
            GetRolePermissions(authRequest.Principal)
                .Values.SelectMany(_rps => _rps)
                .Where(_rps => authRequest.Resources.Contains(_rps.Resource.Name)) //filter by resources found in the permission profile
                .GroupBy(_rps => _rps.Resource.Name)
                .ThrowIf(_rpgroup => _rpgroup.Count() != authRequest.Resources.Count(), "Access Denied") //make sure the user has all the given resources among it's permissions.
                .Select(_rpgroup => _rpgroup.Select(_rp => _rp.Effect).Combine())
                .Combine()
                .ThrowIf(_effect => _effect == Effect.Deny, "Access Denied");
        }

        protected virtual Dictionary<string, IEnumerable<Permission>> GetRolePermissions(User user)
        {
            var roles = _context.Store<UserRole>().Query
                                .Where(_ur => _ur.UserId == user.UserId)
                                .Select(_ur => _ur.RoleName)
                                .ToArray();

            return _context.Store<Permission>()
                           .QueryWith(_p => _p.Role)
                           .Where(_p => roles.Contains(_p.Role.RoleName))
                           .GroupBy(_pgroup => _pgroup.Role.RoleName)
                           .Select(_pgroup => new { Key = _pgroup.Key, Permissions = (IEnumerable<Permission>)_pgroup.ToArray() })
                           .ToDictionary(_pmap => _pmap.Key, _pmap => _pmap.Permissions);
        }
    }
}
