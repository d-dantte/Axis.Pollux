using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Module;
using Axis.Luna.Extensions;
using Axis.Pollux.Account.OAModule.Mappings;
using System;
using System.Linq;

namespace Axis.Pollux.Account.OAModule
{
    public class AccountAccessModuleConfig : BaseModuleConfigProvider
    {
        public override string ModuleName => "Axis.Pollux.Account.ObjectAccessModule";

        protected override void Initialize()
        {
            //Configuration
            var asm = typeof(AccountAccessModuleConfig).Assembly;
            var ns = typeof(ContextVerificationMap).Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap() || t.IsComplexMap())
               .ForAll((cnt, t) => this.UsingConfiguration(Activator.CreateInstance(t).AsDynamic()));
        }
    }
}
