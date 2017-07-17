using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.RoleAuth.Models;
using System.Collections.Generic;

namespace Axis.Pollux.RoleAuth.Services
{
    public interface IRoleManager
    {
        #region Role Management
        IOperation<Role> CreateRole(string roleName);
        IOperation<Role> UpdateRole(Role role);
        IOperation<Role> DisableRole(Role role);
        IOperation<Role> EnableRole(Role role);

        IOperation<IEnumerable<Role>> GetAllRoles();
        #endregion

        #region User Role Management
        IOperation<UserRole> AssignRole(User user, Role role);
        IOperation RevokeRole(User user, Role role);

        IOperation<IEnumerable<UserRole>> GetUserRolesFor(User user);
        #endregion

        #region Permission Management
        IOperation<RolePermission> CreatePermission(Role role, string resource, string label, PermissionEffect effect);
        IOperation<RolePermission> DeletePermission(RolePermission permission);
        IOperation<RolePermission> UpdatePermission(RolePermission permission);

        IOperation<IEnumerable<RolePermission>> GetPermissionsFor(Role role);
        IOperation<IEnumerable<RolePermission>> GetPermissionsForLabel(string label);
        IOperation<IEnumerable<RolePermission>> GetPermissionsForResource(string resource);
        #endregion
    }
}