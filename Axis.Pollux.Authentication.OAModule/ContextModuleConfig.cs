using static Axis.Luna.Extensions.EnumerableExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using Axis.Jupiter.Europa.Module;
using System;
using System.Linq;
using Axis.Pollux.Authentication.OAModule.Mappings;
using Axis.Pollux.Identity.OAModule;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.Authentication.OAModule
{
    public class AuthenticationAccessModuleConfig : BaseModuleConfigProvider
    {
        public override string ModuleName => "Axis.Pollux.Authentication.ObjectAccessModule";

        protected override void Initialize()
        {
            //Configuration
            var asm = typeof(AuthenticationAccessModuleConfig).Assembly;
            var ns = typeof(CredentialMap).Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap() || t.IsComplexMap())
               .ForAll((cnt, t) => this.UsingConfiguration(Activator.CreateInstance(t).AsDynamic()));
        }
    }
}
