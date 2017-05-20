using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Pollux.RBAC.Auth;

namespace Axis.Pollux.RBAC.OAModule.Mappings
{
    public class PermissionMapping: BaseMap<Permission, long>
    {
        public PermissionMapping()
        {
            this.Property(e => e.RoleId)
                .HasMaxLength(400);

            this.HasRequired(_e => _e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId);
        }
    }
}
