using Axis.Luna.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Axis.Pollux.ABAC.AttributeSources
{
    public class OperationIntentMap
    {
        private readonly Dictionary<string, HashSet<AccessIntent>> _actionMap = new Dictionary<string, HashSet<AccessIntent>>();

        public OperationIntentMap Map(string minfo, params AccessIntent[] actionMaps)
        {
            var mapList = _actionMap.GetOrAdd(minfo, _minfo => new HashSet<AccessIntent>());
            mapList.AddRange(actionMaps);
            return this;
        }

        public OperationIntentMap Map<T>(Expression<Action<T>> methodExpression, params AccessIntent[] actionMaps)
        => methodExpression.Body
            .As<MethodCallExpression>()
            .ThrowIfNull("Expression MUST be a simple method call expression")
            .Pipe(mexp => Map(mexp.Method.UniqueSignature(), actionMaps));


        public AccessIntent[] AccessIntentsFor(string methodSignature)
        => _actionMap.ContainsKey(methodSignature) ?
           _actionMap[methodSignature].ToArray() :
           new AccessIntent[0];

        public AccessIntent[] AccessIntentsFor(MethodInfo minfo) => AccessIntentsFor(minfo.UniqueSignature());

        public AccessIntent[] AccessIntentsFor<T>(Expression<Action<T>> methodExpression)
        => methodExpression
            .As<MethodCallExpression>()
            .ThrowIfNull("Expression MUST be a simple method call expression of the form (t => t.Method())")
            .Pipe(mexp => AccessIntentsFor(mexp.Method));
    }
}
