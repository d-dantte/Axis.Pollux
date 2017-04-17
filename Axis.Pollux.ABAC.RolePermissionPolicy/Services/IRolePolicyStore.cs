using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core.Policy;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Sesrvices
{
    public interface IRolePolicyStore : IPolicyReader, IPolicyWriter
    {
        Operation<RolePermission> AddPolicyRule(RolePermission permission);
        Operation<RolePermission> UpdatePolicyRule(RolePermission permission);
        Operation<RolePermission> RemovePolicyRule(RolePermission permission);
        Operation<IEnumerable<RolePermission>> GetPolicyRule(string policy);
        Operation<IEnumerable<RolePermission>> GetUserPolicyRule(User user, string policy);
    }
}
