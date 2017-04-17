using static Axis.Luna.Extensions.EnumerableExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using Axis.Jupiter.Europa.Module;
using System;
using System.Linq;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.EF
{
    public class ABACContextModuleConfig : BaseModuleConfigProvider
    {
        public override string ModuleName => "Axis.Pollux.ABAC.ObjectAccessModule";

        protected override void Initialize()
        {
            //Configuration
            var asm = typeof(ABACContextModuleConfig).Assembly;
            var ns = typeof(Mappings.RolePermissionMapping).Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap() || t.IsComplexMap())
               .ForAll((cnt, t) => this.UsingConfiguration(Activator.CreateInstance(t).AsDynamic()));
        }
    }
}
