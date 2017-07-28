using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using Axis.Sigma.Core;
using System.Runtime.Remoting.Messaging;
using Axis.Luna.Extensions;

using static Axis.Luna.Extensions.ExceptionExtensions;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.ABAC.AttributeSources.Models;

namespace Axis.Pollux.ABAC.AttributeSources.Services
{
    public class CallContextIntentSource : IAttributeSource
    {
        public static readonly string CallContextKey = typeof(CallContextIntentSource).FullName;

        private OperationIntentMap _intentMap = null;

        public CallContextIntentSource(OperationIntentMap map)
        {
            ThrowNullArguments(() => map);

            _intentMap = map;
        }


        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => LazyOp.Try(() =>
        {
            var intent = CallContext
                .LogicalGetData(CallContextKey)
                .Cast<Stack<string>>()?
                .Peek();

            return _intentMap
                .AccessIntentsFor(intent) //<-- will return an empty array if intent is null
                .SelectMany(_ai => new[] { _ai.ActionDescriptor, _ai.ResourceDescriptor });
        });
    }
}
