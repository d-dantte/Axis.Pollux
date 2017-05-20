using Axis.Pollux.ABAC.RolePermissionPolicy.Sesrvices;
using System.Collections.Generic;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core.Policy;
using Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl.Queries;

using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.EnumerableExtensions;
using Axis.Jupiter.Kore.Command;
using System.Linq;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl
{
    public class RolePolicyStore : IRolePolicyStore
    {
        private IRolePermissionQuery _query;
        private IPersistenceCommands _pcommand;

        public RolePolicyStore(IRolePermissionQuery query, IPersistenceCommands pcommand)
        {
            ThrowNullArguments(() => query, () => pcommand);

            _query = query;
            _pcommand = pcommand;
        }


        #region RolePolicyStore

        public Operation<RolePermission> AddPolicyRule(RolePermission permission)
        => Operation.Try(() =>
        {
            return _query.GetRolePermission(permission.PolicyCode, permission.RoleName, permission.IntentDescriptor, permission.Effect) ??
                   _pcommand.Add(permission).Resolve();
        });

        public Operation<IEnumerable<RolePermission>> GetPolicyRules(string policyCode)
        => Operation.Try(() => _query.GetPolicyRules(policyCode));

        public Operation<IEnumerable<RolePermission>> GetUserPolicyRules(User user, string policyCode)
        => Operation.Try(() => _query.GetUserPolicyRules(user.UserId, policyCode));

        public Operation<RolePermission> RemovePolicyRule(RolePermission permission) => _pcommand.Delete(permission);

        public Operation<RolePermission> UpdatePolicyRule(RolePermission permission) => _pcommand.Update(permission);
        #endregion

        #region PolicyIO

        public IEnumerable<Policy> Policies()
        => _query.GetAllPolicyRules()
                 .Select(_rp => new
                 {
                     Code = _rp.PolicyCode,
                     Policy = new Policy
                     {
                         Id = $"{_rp.PolicyCode}.PermissionPolicy/{ValueHash(Enumerate(_rp.PolicyCode))}",
                         CombinationClause = DefaultClauses.GrantOnAll,
                         Rules = new[]
                         {
                             new Rule
                             {
                                 Id = $"{_rp.PolicyCode}.PermissionRule/{ValueHash(Enumerate(_rp.RoleName, _rp.IntentDescriptor))}",
                                 EvaluationFunction = (_r, _ar) =>
                                 {
                                     var policyDescriptor = new IntentDescriptor(_rp.IntentDescriptor);

                                     return _ar.SubjectAttributes().ContainsAttribute(CommonAttributeNames.SubjectAttribute_UserRole, _rp.RoleName) &&
                                            policyDescriptor.IsBaseOf(new IntentDescriptor(_ar.IntentAttributes().GetAttribute(CommonAttributeNames.IntentAttribute_ResourceDescriptor).ResolveData<string>(),
                                                                                           _ar.IntentAttributes().GetAttribute(CommonAttributeNames.IntentAttribute_ActionDescriptor).ResolveData<string>()));
                                 }
                             }
                         },
                         IsAuthRequestTarget = (_p, _ar) => _p.Id == Constants.Misc_RolePermissionPolicyCode
                     }
                 })
                 .GroupBy(_pc => _pc.Code)
                 .Select(_pgroup => new Policy
                 {
                     Id = _pgroup.Key,
                     CombinationClause = DefaultClauses.GrantOnAll,
                     SubPolicies = _pgroup.Select(_pc => _pc.Policy),
                     IsAuthRequestTarget = (_p, _ar) => _p.Id == Constants.Misc_RolePermissionPolicyCode
                 });
        #endregion

    }
}
