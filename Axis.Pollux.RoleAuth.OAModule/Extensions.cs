using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Module;
using Axis.Pollux.RoleAuth.OAModule.Mappings;

namespace Axis.Pollux.RoleAuth.OAModule
{
    public static class Extensions
    {
        public static ContextConfiguration<T> UsingPolluxRoleAuthOAModule<T>(this ContextConfiguration<T> contextConfig)
        where T : DataStore
        {
            var module = new ModuleConfigProvider("Axis.Pollux.RoleAuth.ObjectAccessModule");

            //Configuration
            module.UsingConfiguration(new RoleMap());
            module.UsingConfiguration(new UserRoleMap());
            module.UsingConfiguration(new RolePermissionMap());

            return contextConfig.UsingModule(module);
        }
    }
}
