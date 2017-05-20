using System.Collections.Generic;
using Axis.Sigma.Core.Policy;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl.Queries
{
    public interface IRolePermissionQuery
    {
        RolePermission GetRolePermission(string policyCode, string roleName, string intentDescriptor, Effect effect);
        RolePermission GetRolePermissionById(long id);
        IEnumerable<RolePermission> GetPolicyRules(string policyCode);
        IEnumerable<RolePermission> GetUserPolicyRules(string userId, string policyCode);
        IEnumerable<RolePermission> GetAllPolicyRules();
    }
}
