using Axis.Luna;
using Axis.Sigma.Core.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.ABAC.Services
{
    public interface IPolicyStore : IPolicyReader
    {
        IPolicyExpressionProvider PolicyExpressionProvider { get; }

        Operation<IPolicyStore> DeletePolicy(Auth.PolicySet policySet);
        Operation<IPolicyStore> Modify(Auth.PolicySet policySet);
        Operation<IPolicyStore> Add(Auth.PolicySet policySet);
    }
}
