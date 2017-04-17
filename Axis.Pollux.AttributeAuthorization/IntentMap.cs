using Axis.Luna.Extensions;
using Axis.Sigma.Core.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Axis.Pollux.AttributeAuthorization
{
    public class IntentMap
    {
        private readonly Dictionary<MethodInfo, HashSet<AccessIntent>> _actionMap = new Dictionary<MethodInfo, HashSet<AccessIntent>>();

        public IntentMap Map(MethodInfo minfo, params AccessIntent[] actionMaps)
        {
            var mapList = _actionMap.GetOrAdd(minfo, _minfo => new HashSet<AccessIntent>());
            mapList.AddRange(actionMaps);
            return this;
        }

        public IntentMap Map<T>(Expression<Action<T>> methodExpression, params AccessIntent[] actionMaps)
        => methodExpression.Body
            .As<MethodCallExpression>()
            .ThrowIfNull("Expression MUST be a simple method call expression")
            .Pipe(mexp => Map(mexp.Method, actionMaps));


        public AccessIntent[] ResourceDescriptorsFor(MethodInfo minfo)
            => _actionMap.ContainsKey(minfo) ?
               _actionMap[minfo].ToArray() :
               new AccessIntent[0];

        public AccessIntent[] ResourceDescriptorsFor<T>(Expression<Action<T>> methodExpression)
        => methodExpression
            .As<MethodCallExpression>()
            .ThrowIfNull("Expression MUST be a simple method call expression")
            .Pipe(mexp => ResourceDescriptorsFor(mexp.Method));
    }
}
