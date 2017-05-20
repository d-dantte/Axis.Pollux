using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Mappings;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.EF.Mappings;
{
    public class UserRoleMapping : BaseMap<UserRole, long>
    {
        public UserRoleMapping()
        {
            //properties
            Property(ur => ur.UserId)
                .HasMaxLength(250)
                .IsIndex("OwnerIdentityIndex");

            Property(ur => ur.RoleName)
                .HasMaxLength(250);
        }
    }
}
