using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.Auth
{
    public class EmptyAttributeSource : IAttributeSource
    {
        public IOperation<IEnumerable<IAttribute>> GetAttributes() => LazyOp.Try(() => new IAttribute[0].AsEnumerable());
    }
}
