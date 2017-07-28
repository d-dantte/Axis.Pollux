using Axis.Luna;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.RoleAuth.Models;
using Axis.Pollux.Common.Models;
using System;
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

        IOperation<SequencePage<Role>> GetAllRoles(PageParams pageParams = null);
        #endregion

        #region User Role Management
        IOperation<UserRole> AssignRole(User user, Role role);
        IOperation RevokeRole(User user, Role role);

        IOperation<SequencePage<UserRole>> GetUserRolesFor(User user, PageParams pageParams = null);
        #endregion

        #region Permission Management
        IOperation<RolePermission> CreatePermission(Role role, string resource, string label, PermissionEffect effect);
        IOperation<RolePermission> DeletePermission(RolePermission permission);
        IOperation<RolePermission> UpdatePermission(RolePermission permission);

        IOperation<RolePermission> GetPermissionForUUID(Guid uuid);
        IOperation<SequencePage<RolePermission>> GetPermissionsFor(Role role, PageParams pageParams = null);
        IOperation<SequencePage<RolePermission>> GetPermissionsForLabel(string label, PageParams pageParams = null);
        IOperation<SequencePage<RolePermission>> GetPermissionsForResource(string resource, PageParams pageParams = null);
        #endregion
    }
}