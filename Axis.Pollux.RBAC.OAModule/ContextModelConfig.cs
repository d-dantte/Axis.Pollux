using static Axis.Luna.Extensions.EnumerableExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using Axis.Jupiter.Europa.Module;
using Axis.Pollux.RBAC.OAModule.Mappings;
using System;
using System.Linq;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.RBAC.OAModule
{
    public class RBACAccessModuleConfig : BaseModuleConfigProvider
    {
        public override string ModuleName => "Axis.Pollux.RBAC.ObjectAccessModule";

        protected override void Initialize()
        {
            //Configuration
            var asm = typeof(RBACAccessModuleConfig).Assembly;
            var ns = typeof(RoleMapping).Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap())
               .ForAll((cnt, t) => this.UsingConfiguration(Activator.CreateInstance(t).AsDynamic()));

            //seeding
            //nothing to seed for now
        }
    }
}
