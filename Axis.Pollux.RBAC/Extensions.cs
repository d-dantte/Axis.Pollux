using System.Collections.Generic;
using System.Linq;
using Axis.Pollux.RoleAuth.Models;

namespace Axis.Pollux.RBAC
{
    public static class Extensions
    {

        public static PermissionEffect Combine(this IEnumerable<PermissionEffect> effects)
        {
            if (effects == null) return PermissionEffect.Deny;
            else return effects.Aggregate(PermissionEffect.Grant, (effect, next) =>
            {
                if (effect == PermissionEffect.Deny) return PermissionEffect.Deny;
                else return next;
            });
        }
    }
}
