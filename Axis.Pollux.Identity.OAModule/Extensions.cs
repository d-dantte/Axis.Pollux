using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Module;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.OAModule.Mappings;
using System;
using System.Linq;

namespace Axis.Pollux.Identity.OAModule
{
    public static class Extensions
    {
        public static ContextConfiguration<T> UsingPolluxIdentityOAModule<T>(this ContextConfiguration<T> contextConfig)
        where T: DataStore
        {
            var module = new ModuleConfigProvider("Axis.Pollux.Identity.ObjectAccessModule");

            //Configuration
            var asm = typeof(Extensions).Assembly;
            var ns = typeof(UserMap).Namespace;
            asm.GetTypes()
               .Where(t => t.Namespace == ns)
               .Where(t => t.IsEntityMap())
               .Select(Activator.CreateInstance)
               .Select(ObjectExtensions.AsDynamic)
               .ForAll(mapping => module.UsingConfiguration(mapping));

            return contextConfig.UsingModule(module);
        }
    }
}
