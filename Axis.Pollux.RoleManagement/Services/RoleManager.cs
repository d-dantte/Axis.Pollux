using Axis.Jupiter.Commands;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.RoleAuth.Models;
using Axis.Pollux.RoleAuth.Services;
using Axis.Pollux.RoleManagement.Queries;
using System;
using System.Collections.Generic;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Pollux.Identity.Extensions;

namespace Axis.Pollux.RoleManagement.Services
{
    public class RoleManager: IRoleManager
    {
        #region Role Management
        public IOperation<Role> CreateRole(string roleName)
        => LazyOp.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(roleName)) throw new Exception("invalid role name");

            var role = new Role { RoleName = roleName };
            return _pcommands.Add(role);
        });

        public IOperation<Role> UpdateRole(Role role)
        => ValidateModels(role)
            .Then(() => _pcommands.Update(role));

        public IOperation<Role> DisableRole(Role role)
        => ValidateModels(role)
            .Then(() =>
            {
                role.Status = RoleStatus.Disabled;
                return _pcommands.Update(role);
            });

        public IOperation<Role> EnableRole(Role role)
        => ValidateModels(role)
            .Then(() =>
            {
                role.Status = RoleStatus.Enabled;
                return _pcommands.Update(role);
            });

        public IOperation<IEnumerable<Role>> GetAllRoles()
        => LazyOp.Try(() =>
        {
            return _queries.GetAllRoles();
        });
        #endregion

        #region User Role Management
        public IOperation<UserRole> AssignRole(User user, Role role)
        => ValidateModels(user, role)
            .Then(() =>
            {
                var userRole = new UserRole
                {
                    Role = role,
                    User = user
                };
                return _pcommands.Add(userRole);
            });

        public IOperation RevokeRole(User user, Role role)
        => ValidateModels(user, role)
            .Then(() =>
            {
                var userRole = _queries
                    .GetUserRole(user.UserId, role.RoleName)
                    .ThrowIfNull("user role does not exist");

                _pcommands
                    .Delete(userRole)
                    .Resolve();
            });

        public IOperation<IEnumerable<UserRole>> GetUserRolesFor(User user)
        => ValidateModels(user)
            .Then(() =>
            {
                return _queries.GetUserRolesFor(user.UserId);
            });
        #endregion

        #region Permission Management
        public IOperation<RolePermission> CreatePermission(Role role, string resource, string label, PermissionEffect effect)
        => ValidateModels(role)
            .Then(() =>
            {
                if (string.IsNullOrWhiteSpace(resource)) throw new Exception("invalid resource");
                else if (string.IsNullOrWhiteSpace(label)) throw new Exception("invalid label");

                var rolePermission = new RolePermission
                {
                    Effect = effect,
                    Label = label,
                    Resource = resource,
                    Role = role
                };

                return _pcommands.Add(rolePermission);
            });

        public IOperation<RolePermission> DeletePermission(RolePermission permission)
        => ValidateModels(permission)
            .Then(() =>
            {
                return _pcommands.Delete(permission);
            });

        public IOperation<RolePermission> UpdatePermission(RolePermission permission)
        => ValidateModels(permission)
            .Then(() =>
            {
                return _pcommands.Update(permission);
            });

        public IOperation<IEnumerable<RolePermission>> GetPermissionsFor(Role role)
        => ValidateModels(role)
            .Then(() =>
            {
                return _queries.GetPermissionsForRole(role.RoleName);
            });

        public IOperation<IEnumerable<RolePermission>> GetPermissionsForLabel(string label)
        => LazyOp.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(label)) throw new Exception("invalid label");
            return _queries.GetPermissionsForLabel(label);
        });

        public IOperation<IEnumerable<RolePermission>> GetPermissionsForResource(string resource)
        => LazyOp.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(resource)) throw new Exception("invalid resource");
            return _queries.GetPermissionsForResource(resource);
        });

        public IOperation<RolePermission> GetPermissionForUUID(Guid uuid)
        => LazyOp.Try(() =>
        {
            return _queries.GetPermissionForUUID(uuid);
        });
        #endregion


        #region init
        public RoleManager(IPersistenceCommands pcommands, IRoleManagementQueries queries)
        {
            ThrowNullArguments(() => pcommands, () => queries);

            _queries = queries;
            _pcommands = pcommands;
        }
        #endregion

        #region Locals
        private IPersistenceCommands _pcommands = null;
        private IRoleManagementQueries _queries = null;
        #endregion
    }
}
