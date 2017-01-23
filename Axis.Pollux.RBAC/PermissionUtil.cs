using Axis.Pollux.RBAC.Auth;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Pollux.RBAC
{
    public static class PermissionUtil
    {
        public static Effect Combine(this Effect source, Effect value)
            => source == Effect.Deny || value == Effect.Deny ? Effect.Deny : Effect.Grant;

        public static Effect Combine(this IEnumerable<Effect> values)
            => values.Aggregate((_prev, _curr) => _prev.Combine(_curr));

        public static Effect Combine(this Permission source, Permission value) => source.Effect.Combine(value.Effect);
    }
}
