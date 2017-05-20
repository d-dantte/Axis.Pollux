using static Axis.Luna.Extensions.EnumerableExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using Axis.Jupiter.Europa.Module;
using Axis.Pollux.Identity.OAModule.Mappings;
using System;
using System.Linq;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.Identity.OAModule
{
    public class IdentityAccessModuleConfig : BaseModuleConfigProvider
    {
        public override string ModuleName => "Axis.Pollux.Identity.ObjectAccessModule";

        protected override void Initialize()
        {
            //Configuration
            var asm = typeof(IdentityAccessModuleConfig).Assembly;
            var bmt = typeof(BaseMap<,>);
            var ns = bmt.Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap())
               .ForAll((cnt, t) => this.UsingConfiguration(Activator.CreateInstance(t).AsDynamic()));
        }
    }
}
