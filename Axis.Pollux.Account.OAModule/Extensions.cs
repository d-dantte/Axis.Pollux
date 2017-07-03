using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Module;
using Axis.Luna.Extensions;
using Axis.Pollux.AccountManagement.OAModule.Mappings;
using System;
using System.Linq;

namespace Axis.Pollux.AccountManagement.OAModule
{
    public static class Extensions
    {
        public static ContextConfiguration<T> UsingPolluxAccountOAModule<T>(this ContextConfiguration<T> contextConfig)
        where T : DataStore
        {
            var module = new ModuleConfigProvider("Axis.Pollux.Account.ObjectAccessModule");

            //Configuration
            var asm = typeof(Extensions).Assembly;
            var ns = typeof(ContextVerificationMap).Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap() || t.IsComplexMap())
               .Select(Activator.CreateInstance)
               .Select(ObjectExtensions.AsDynamic)
               .ForAll(mapping => module.UsingConfiguration(mapping));

            return contextConfig.UsingModule(module);
        }
    }
}
