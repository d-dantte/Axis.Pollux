using Axis.Jupiter.Commands;
using Axis.Luna.Operation;
using Axis.Pollux.ABAC.RolePermissionPolicy.Queries;
using Axis.Sigma.Core.Policy;
using System.Collections.Generic;
using System.Linq;
using System;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.EnumerableExtensions;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services
{
    public class RolePolicyStore : IPolicyReader, IPolicyWriter
    {
        private IPrincipalRoleQuery _query;
        private IPersistenceCommands _pcommand;

        public RolePolicyStore(IPrincipalRoleQuery query, IPersistenceCommands pcommand)
        {
            ThrowNullArguments(() => query, () => pcommand);

            _query = query;
            _pcommand = pcommand;
        }

        
        public IOperation<RolePermission> AddPolicyRule(RolePermission permission)
        => LazyOp.Try(() =>
        {
            return _query.GetRolePermission(permission.PolicyCode, permission.RoleName, permission.IntentDescriptor, permission.Effect) ??
                   _pcommand.Add(permission).Resolve();
        });

        public IOperation<RolePermission> RemovePolicyRule(RolePermission permission) => _pcommand.Delete(permission);

        public IOperation<RolePermission> UpdatePolicyRule(RolePermission permission) => _pcommand.Update(permission);

        #region PolicyIO

        public IOperation<IEnumerable<Policy>> Policies()
        => LazyOp.Try(() => _query.GetAllPolicyRules()
                 .Select(_rp =>
                 {
                     var policyId = $"{_rp.PolicyCode}.PermissionPolicy[{ValueHash(Enumerate(_rp.PolicyCode))}]";
                     var ruleId = $"{_rp.PolicyCode}.PermissionRule[{ValueHash(Enumerate(_rp.RoleName, _rp.IntentDescriptor))}]";
                     var policyDescriptor = new IntentDescriptor(_rp.IntentDescriptor);
                     return new
                     {
                         PolicyId = policyId,
                         Rule = new Rule
                         {
                             Id = _rp.PermissionGuid,
                             Code = ruleId,
                             EvaluationFunction = (_r, _ar) =>
                             {
                                 return _ar.SubjectAttributes().ContainsAttribute(CommonAttributeNames.SubjectAttribute_UserRole, _rp.RoleName) &&
                                        policyDescriptor.IsBaseOf(new IntentDescriptor(_ar.IntentAttributes().GetAttribute(CommonAttributeNames.IntentAttribute_ResourceDescriptor).ResolveData<string>(),
                                                                                       _ar.IntentAttributes().GetAttribute(CommonAttributeNames.IntentAttribute_ActionDescriptor).ResolveData<string>()));
                             }
                         }
                     };
                 })
                 .GroupBy(_pc => _pc.PolicyId)
                 .Select(_pgroup =>
                 {
                     return new Policy
                     {
                         Code = _pgroup.Key,
                         CombinationClause = DefaultClauses.GrantOnAll,
                         Rules = _pgroup.Select(_pc => _pc.Rule),
                         IsAuthRequestTarget = (_p, _ar) => _ar.SubjectAttributes().ContainsAttribute(CommonAttributeNames.SubjectAttribute_UserRole)
                     };
                 }));


        public IOperation Persist(IEnumerable<Policy> policies)
        => LazyOp.Try(() =>
        {
            policies.SelectMany(_p => _p.Rules) //expecting no subpolicies
                    .Select(_r => new RolePermission
                    {
                        ModifiedOn = DateTime.Now,
                        Effect = _r.eff
                    })
                    .Select(_r => new { Exists = _query.GetRolePermissionByGuid(_r.PermissionGuid) != null, _r })
                    .Pipe(_rarr =>
                    {
                        //insert?
                        _rarr.Where(__r => !__r.Exists)
                             .Pipe(_ => _pcommand.AddBatch(_))
                             .Resolve();

                        //update
                        _rarr.Where(__r => __r.Exists)
                             .Pipe(_ => _pcommand.UpdateBatch(_))
                             .Resolve();
                    });
        });

        #endregion

    }
}
