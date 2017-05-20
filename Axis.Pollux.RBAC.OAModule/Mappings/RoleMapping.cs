using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Pollux.RBAC.Auth;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.RBAC.OAModule.Mappings
{
    public class RoleMapping: BaseMap<Role, string>
    {
        public RoleMapping()
        {
            //properties
            Property(r => r.EntityId).HasMaxLength(250);
            Property(r => r.RoleName).HasMaxLength(250).IsIndex(nameof(Role.RoleName), true);

            //ignores
            this.Ignore(r => r.Value)
                .Ignore(r => r.Name);
        }
    }
}
