using Axis.Luna.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Axis.Pollux.ABAC.DAS.Models
{
    public class OperationIntentMap
    {
        private readonly Dictionary<string, HashSet<AccessIntent>> _actionMap = new Dictionary<string, HashSet<AccessIntent>>();

        public OperationIntentMap Map(MethodInfo minfo, params AccessIntent[] intentMaps)
        => minfo
            .ThrowIfNull("Invalid method info")
            .Pipe(_minfo => Map(_minfo.UniqueSignature(), intentMaps));

        public OperationIntentMap Map<T>(Expression<Action<T>> methodExpression, params AccessIntent[] intentMaps)
        => methodExpression.Body
            .Cast<MethodCallExpression>()
            .ThrowIfNull("Expression MUST be a simple method call expression")
            .Pipe(mexp => Map(mexp.Method.UniqueSignature(), intentMaps));

        private OperationIntentMap Map(string minfo, params AccessIntent[] actionMaps)
        {
            var mapList = _actionMap.GetOrAdd(minfo, _minfo => new HashSet<AccessIntent>());
            mapList.AddRange(actionMaps);
            return this;
        }


        public AccessIntent[] AccessIntentsFor(string methodSignature)
        => _actionMap.ContainsKey(methodSignature) ?
           _actionMap[methodSignature].ToArray() :
           new AccessIntent[0];

        public AccessIntent[] AccessIntentsFor(MethodInfo minfo) => AccessIntentsFor(minfo.UniqueSignature());

        public AccessIntent[] AccessIntentsFor<T>(Expression<Action<T>> methodExpression)
        => methodExpression
            .Cast<MethodCallExpression>()
            .ThrowIfNull("Expression MUST be a simple method call expression of the form (t => t.Method())")
            .Pipe(mexp => AccessIntentsFor(mexp.Method));
    }
}
