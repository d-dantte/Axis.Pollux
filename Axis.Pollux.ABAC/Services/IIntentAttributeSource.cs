using Axis.Luna;
using Axis.Sigma.Core;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.Services
{
    public interface IIntentAttributeSource
    {
        Operation<IEnumerable<IAttribute>> GetAttributes();
    }
}
