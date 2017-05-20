using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class UserDataMap: BaseMap<UserData, long>
    {
        public UserDataMap()
        {
            ///Conifgure Relationships.

            //one way to owner
            this.HasRequired(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId);

            this.Property(e => e.Name)
                .HasMaxLength(400)
                .IsIndex("UserDataName");
        }
    }
}
