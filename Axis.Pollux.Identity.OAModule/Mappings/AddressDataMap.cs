using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class AddressDataMap: BaseMap<AddressData, long>
    {
        public AddressDataMap()
        {
            ///Configure Properties
            this.Property(e => e.City).HasMaxLength(250);
            this.Property(e => e.Country).HasMaxLength(250);
            this.Property(e => e.StateProvince).HasMaxLength(250);
            this.Property(e => e.Street).HasMaxLength(250);

            ///Conifgure Relationships.            
            this.HasRequired(e => e.Owner)//one way to owner
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }
    }
}
