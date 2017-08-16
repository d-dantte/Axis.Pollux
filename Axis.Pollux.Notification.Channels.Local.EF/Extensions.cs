using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Module;
using Axis.Luna.Extensions;
using System;
using System.Linq;

namespace Axis.Pollux.Notification.Client.EF
{
    public static class Extensions
    {
        public static ContextConfiguration<T> UsingPolluxClientNotificationModule<T>(this ContextConfiguration<T> contextConfig)
        where T : DataStore
        {
            var module = new ModuleConfigProvider("Axis.Pollux.Notifications.ClientChannel.ObjectAccessModule");

            //Configuration
            var asm = typeof(Extensions).Assembly;
            var ns = typeof(Mappings.NotificationMapping).Namespace;
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
