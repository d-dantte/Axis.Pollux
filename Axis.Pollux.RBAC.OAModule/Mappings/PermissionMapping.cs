using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Pollux.RBAC.Auth;

namespace Axis.Pollux.RBAC.OAModule.Mappings
{
    public class PermissionMapping: BaseMap<Permission, long>
    {
        public PermissionMapping()
        {
            this.HasRequired(_e => _e.Role)
                .WithMany();
        }
    }
}
