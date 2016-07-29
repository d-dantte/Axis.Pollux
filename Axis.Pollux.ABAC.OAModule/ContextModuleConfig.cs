using static Axis.Luna.Extensions.EnumerableExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using Axis.Jupiter.Europa.Module;
using System;
using System.Linq;
using Axis.Pollux.ABAC.OAModule.Mappings;
using Axis.Pollux.Identity.OAModule;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.ABAC.OAModule
{
    public class ABACContextModuleConfig : BaseModuleConfigProvider
    {
        public override string ModuleName => "Axis.Pollux.ABAC.ObjectAccessModule";

        protected override void Initialize()
        {
            //Configuration
            var asm = typeof(ABACContextModuleConfig).Assembly;
            var ns = typeof(AttributeMapping).Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap() || t.IsComplexMap())
               .ForAll((cnt, t) => this.UsingConfiguration(Activator.CreateInstance(t).AsDynamic()));

            //seeding
            //nothing to seed for now
        }
    }
}
