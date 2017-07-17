using Axis.Pollux.RoleAuth.Models;
using System.Collections.Generic;

namespace Axis.Pollux.RoleManagement.Queries
{
    public interface IRoleManagementQueries
    {
        IEnumerable<Role> GetAllRoles();
        UserRole GetUserRole(string userId, string roleName);
        IEnumerable<UserRole> GetUserRolesFor(string userId);
        IEnumerable<RolePermission> GetPermissionsForRole(string roleName);
        IEnumerable<RolePermission> GetPermissionsForLabel(string label);
        IEnumerable<RolePermission> GetPermissionsForResource(string resource);
    }
}
