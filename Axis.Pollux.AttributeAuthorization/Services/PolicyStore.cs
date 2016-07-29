using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna.Extensions;

using Axis.Pollux.ABAC.Services;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.ABAC;
using Axis.Jupiter;

namespace Axis.Pollux.AttributeAuthorization.Services
{
    public class PolicyStore : IPolicyStore
    {
        private IDataContext _context { get; set; }

        public IQueryable<Sigma.Core.Policy.PolicySet> Policies
            => _context.Store<ABAC.Auth.PolicySet>().Query
                       .AsEnumerable()
                       .Select(ps => ps.ToSigma(PolicyExpressionProvider))
                       .AsQueryable();

        public PolicyStore(IPolicyExpressionProvider expressionProvider)
        {
            ThrowNullArguments(() => expressionProvider);

            this.PolicyExpressionProvider = expressionProvider;
        }

        public IPolicyExpressionProvider PolicyExpressionProvider { get; private set; }

        public Operation<IPolicyStore> DeletePolicy(ABAC.Auth.PolicySet policySet)
            => Operation.Run(() =>
            {
                if (policySet != null)
                {
                    //delete the entire hierarchy of policies
                    policySet.Policies.ForAll((cnt, policy) =>
                    {
                        policy.Rules.ForAll((cntt, rule) => _context.Store<ABAC.Auth.Rule>().Delete(rule));

                        _context.Store<ABAC.Auth.Policy>().Delete(policy);
                    });

                    policySet.SubSets.ForAll((cnt, set) => DeletePolicy(policySet));

                    _context.Store<ABAC.Auth.PolicySet>().Delete(policySet);
                    _context.CommitChanges();
                }

                return this.As<IPolicyStore>();
            });

        public Operation<IPolicyStore> Modify(ABAC.Auth.PolicySet policySet)
            => Operation.Run(() =>
            {
                _context.Store<ABAC.Auth.PolicySet>().Modify(policySet);
                _context.CommitChanges();

                return this.As<IPolicyStore>();
            });

        public Operation<IPolicyStore> Add(ABAC.Auth.PolicySet policySet)
            => Operation.Run(() =>
            {
                _context.Store<ABAC.Auth.PolicySet>().Add(policySet);
                _context.CommitChanges();

                return this.As<IPolicyStore>();
            });
    }
}
