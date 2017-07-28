using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Module;
using Axis.Pollux.Audit.OAModule.Mappings;

namespace Axis.Pollux.Audit.OAModule
{
    public static class Extensions
    {
        public static ContextConfiguration<T> UsingPolluxAuditOAModule<T>(this ContextConfiguration<T> contextConfig)
        where T : DataStore
        {
            var module = new ModuleConfigProvider("Axis.Pollux.Audit.ObjectAccessModule");

            //Configuration
            module.UsingConfiguration(new AuditEntryMapping());

            return contextConfig.UsingModule(module);
        }
    }
}
