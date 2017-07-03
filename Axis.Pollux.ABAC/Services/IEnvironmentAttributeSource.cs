using Axis.Luna.Operation;
using Axis.Sigma.Core;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.Services
{
    public interface IEnvironmentAttributeSource
    {
        IOperation<IEnumerable<IAttribute>> GetAttributes();
    }
}
