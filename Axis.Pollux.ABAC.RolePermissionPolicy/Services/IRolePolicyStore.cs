using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core.Policy;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Sesrvices
{
    public interface IRolePolicyStore : IPolicyReader
    {
        IOperation<RolePermission> AddPolicyRule(RolePermission permission);
        IOperation<RolePermission> UpdatePolicyRule(RolePermission permission);
        IOperation<RolePermission> RemovePolicyRule(RolePermission permission);
        IOperation<IEnumerable<RolePermission>> GetPolicyRules(string policy);
        IOperation<IEnumerable<RolePermission>> GetUserPolicyRules(User user, string policy);
    }
}
