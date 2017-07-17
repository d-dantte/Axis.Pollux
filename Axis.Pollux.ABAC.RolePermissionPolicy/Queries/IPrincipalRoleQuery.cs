using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core.Policy;
using System;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Queries
{
    public interface IPrincipalRoleQuery
    {
        IEnumerable<UserRole> GetUserRoles(string userId);
        UserRole GetUserRole(string userId, string roleName);
        User GetUser(string userId);
        RolePermission GetRolePermission(string policyCode, string roleName, string intentDescriptor, Effect effect);
        RolePermission GetRolePermissionByGuid(Guid guid);
        IEnumerable<RolePermission> GetAllPolicyRules();
    }
}
