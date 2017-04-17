using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Mappings;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.EF.Mappings
{
    public class RolePermissionMapping: BaseMap<RolePermission,long>
    {
        public RolePermissionMapping()
        {
            this.Property(p => p.RoleName)
                .HasMaxLength(250)
                .IsIndex($"{nameof(RolePermission)}_{nameof(RolePermission.RoleName)}");
                        
            this.Property(p => p.ResourceSelector)
                .IsMaxLength();
        }
    }
}
