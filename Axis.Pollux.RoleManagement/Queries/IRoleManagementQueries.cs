using Axis.Luna;
using Axis.Pollux.RoleAuth.Models;
using Axis.Pollux.UserCommon.Models;
using System;
using System.Collections.Generic;

namespace Axis.Pollux.RoleManagement.Queries
{
    public interface IRoleManagementQueries
    {
        SequencePage<Role> GetAllRoles(PageParams pageParams = null);
        UserRole GetUserRole(string userId, string roleName);
        SequencePage<UserRole> GetUserRolesFor(string userId, PageParams pageParams = null);
        RolePermission GetPermissionForUUID(Guid uuid);
        SequencePage<RolePermission> GetPermissionsForRole(string roleName, PageParams pageParams = null);
        SequencePage<RolePermission> GetPermissionsForLabel(string label, PageParams pageParams = null);
        SequencePage<RolePermission> GetPermissionsForResource(string resource, PageParams pageParams = null);
    }
}
