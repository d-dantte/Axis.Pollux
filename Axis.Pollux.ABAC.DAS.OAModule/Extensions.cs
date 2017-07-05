using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Module;
using Axis.Pollux.ABAC.DAS.OAModule.Mappings;

namespace Axis.Pollux.ABAC.DAS.OAModule
{
    public static class Extensions
    {
        public static ContextConfiguration<T> UsingPolluxABACRolePermissionOAModule<T>(this ContextConfiguration<T> contextConfig)
        where T : DataStore
        {
            var module = new ModuleConfigProvider("Axis.Pollux.ABAC.RolePermissionSource.ObjectAccessModule");

            //Configuration
            module.UsingConfiguration(new UserRoleMap());
            module.UsingConfiguration(new RolePermissionMap());

            return contextConfig.UsingModule(module);
        }
    }
}
