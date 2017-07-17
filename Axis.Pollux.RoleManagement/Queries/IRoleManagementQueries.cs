using Axis.Pollux.RoleAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
