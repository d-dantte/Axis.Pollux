using Axis.Luna;
using Axis.Sigma.Core.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.ABAC.Services
{
    public interface IPolicyStore : IPolicyReader, IPolicyWriter
    {
        Operation<IPolicyStore> DeletePolicy(Auth.PolicySet policySet, bool commit = true);
        Operation<IPolicyStore> Modify(Auth.PolicySet policySet);
    }
}
