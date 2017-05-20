using Axis.Luna;
using Axis.Sigma.Core;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.Services
{
    public interface IUserAttributeSource
    {
        Operation<IEnumerable<IAttribute>> GetAttributes();
    }
}
