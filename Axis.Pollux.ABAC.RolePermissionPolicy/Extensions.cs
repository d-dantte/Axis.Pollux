using Axis.Sigma.Core;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Pollux.ABAC.RolePermissionPolicy
{
    public static class Extensions
    {
        public static bool ContainsAttribute<V>(this IEnumerable<IAttribute> @this, string attributeName, V value)
        => @this.Where(_att => _att.Name == attributeName)
                .Where(_att => value?.Equals(_att.ResolveData<V>()) == true)
                .Any();

        public static IAttribute GetAttribute(this IEnumerable<IAttribute> @this, string attributeName) => @this.FirstOrDefault(_att => _att.Name == attributeName);
    }
}
