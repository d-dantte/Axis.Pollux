using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using Axis.Luna;
using Axis.Sigma.Core;
using System.Runtime.Remoting.Messaging;
using Axis.Luna.Extensions;
using Axis.Pollux.ABAC.AttributeSources;

using static Axis.Luna.Extensions.ExceptionExtensions;
using System.Linq;

namespace Axis.Pollux.ABAC.OperationIntentSource
{
    public class CallContextIntentSource : IIntentAttributeSource
    {
        public static readonly string CallContextKey = typeof(CallContextIntentSource).FullName;

        private OperationIntentMap _intentMap = null;

        public CallContextIntentSource(OperationIntentMap map)
        {
            ThrowNullArguments(() => map);

            _intentMap = map;
        }


        public Operation<IEnumerable<IAttribute>> GetAttributes()
        => Operation.Try(() =>
        {
            var intent = CallContext.LogicalGetData(CallContextKey)
                                    .As<Stack<string>>()?
                                    .Peek();

            return _intentMap
                .AccessIntentsFor(intent) //<-- will return an empty array if intent is null
                .SelectMany(_ai => new[] { _ai.ActionDescriptor, _ai.ResourceDescriptor });
        });
    }
}
