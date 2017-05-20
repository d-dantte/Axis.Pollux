using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core.Policy;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Sesrvices
{
    public interface IRolePolicyStore : IPolicyReader
    {
        Operation<RolePermission> AddPolicyRule(RolePermission permission);
        Operation<RolePermission> UpdatePolicyRule(RolePermission permission);
        Operation<RolePermission> RemovePolicyRule(RolePermission permission);
        Operation<IEnumerable<RolePermission>> GetPolicyRules(string policy);
        Operation<IEnumerable<RolePermission>> GetUserPolicyRules(User user, string policy);
    }
}
