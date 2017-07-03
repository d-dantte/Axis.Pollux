using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using Axis.Luna.Operation;
using Axis.Sigma.Core;
using Axis.Pollux.ABAC.DAS.Models;
using System.Reflection;
using System.Linq;

namespace Axis.Pollux.ABAC.DAS.Services
{
    public class IntentMapSource : IIntentAttributeSource
    {
        private IOperation<IEnumerable<IAttribute>> _intentAttributes;

        public IntentMapSource(OperationIntentMap map, MethodInfo operation)
        {
            _intentAttributes =
                LazyOp.Try(() => map
                                 .AccessIntentsFor(operation)
                                 .SelectMany(_ai => new[] { _ai.ActionDescriptor, _ai.ResourceDescriptor }));
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes() => _intentAttributes;
    }
}
