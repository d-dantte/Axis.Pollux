using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using Axis.Luna.Operation;
using Axis.Sigma.Core;
using Axis.Pollux.ABAC.AttributeSources.Models;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System;
using Axis.Luna.Extensions;

namespace Axis.Pollux.ABAC.AttributeSources.Services
{
    public class IntentMapSource : IAttributeSource
    {
        private IOperation<IEnumerable<IAttribute>> _intentAttributes;

        public IntentMapSource(OperationIntentMap map, MethodInfo operation)
        {
            _intentAttributes = LazyOp.Try(() => map
                .AccessIntentsFor(operation)
                .SelectMany(_ai => new[] { _ai.ActionDescriptor, _ai.ResourceDescriptor }));
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes() => _intentAttributes;
    }

    public class IntentMapSource<T>: IntentMapSource
    {
        public IntentMapSource(OperationIntentMap map, Expression<Action<T>> methodExpression)
        : base(map, methodExpression.Body
                                    .Cast<MethodCallExpression>()
                                    .ThrowIfNull("Expression MUST be a simple method call expression")
                                    .Method)
        { }
    }
}
