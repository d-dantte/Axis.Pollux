using static Axis.Luna.Extensions.EnumerableExtensions;

using Axis.Jupiter.Europa.Module;
using System;
using System.Linq;
using Axis.Pollux.Authentication.OAModule.Mappings;
using Axis.Jupiter.Europa;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Authentication.OAModule
{
    public static class Extensions
    {
        public static ContextConfiguration<T> UsingPolluxAuthenticationOAModule<T>(this ContextConfiguration<T> contextConfig)
        where T : DataStore
        {
            var module = new ModuleConfigProvider("Axis.Pollux.Authentication.ObjectAccessModule");

            //Configuration
            var asm = typeof(Extensions).Assembly;
            var ns = typeof(CredentialMap).Namespace;
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
