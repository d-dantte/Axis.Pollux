using Axis.Luna.Operation;
using Axis.Sigma.Core.Policy;
using System.Collections.Generic;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.EnumerableExtensions;
using Axis.Pollux.RoleAuth.Services;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services
{
    public class RolePolicyReader : IPolicyReader
    {
        private IRoleManager _manager;

        public RolePolicyReader(IRoleManager manager)
        {
            ThrowNullArguments(() => manager);

            _manager = manager;
        }

        #region PolicyIO

        public IOperation<IEnumerable<Policy>> Policies()
        => _manager
            .GetAllRoles()
            .Then(_roles => _roles.Page
            .SelectMany(_role => _manager.GetPermissionsFor(_role).Resolve().Page)
            .Select(_rp =>
            {
                var policyId = $"{_rp.Label ?? "$$Default"}.PermissionPolicy[{ValueHash(Enumerate(_rp.Label))}]";
                var ruleId = $"{_rp.Label ?? "$$Default"}.PermissionRule[{ValueHash(Enumerate(_rp.Role.RoleName, _rp.Resource))}]";
                var policyDescriptor = new IntentDescriptor(_rp.Resource);
                return new
                {
                    PolicyId = policyId,
                    Rule = new Rule
                    {
                        Id = _rp.UUID,
                        Code = ruleId,
                        EvaluationFunction = (_r, _ar) =>
                        {
                            return _ar.SubjectAttributes().ContainsAttribute(Constants.SubjectAttribute_UserRole, _rp.Role.RoleName) &&
                                   policyDescriptor.IsBaseOf(new IntentDescriptor(_ar.IntentAttributes().GetAttribute(Constants.IntentAttribute_ResourceDescriptor).ResolveData<string>(),
                                                                                  _ar.IntentAttributes().GetAttribute(Constants.IntentAttribute_ActionDescriptor).ResolveData<string>())) &&
                                   _rp.Effect == RoleAuth.Models.PermissionEffect.Grant;
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
                    IsAuthRequestTarget = (_p, _ar) => _ar.SubjectAttributes().ContainsAttribute(Constants.SubjectAttribute_UserRole)
                };
            }));
        #endregion

    }
}
