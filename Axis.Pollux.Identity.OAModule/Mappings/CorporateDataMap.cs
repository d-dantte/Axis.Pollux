using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class CorporateDataMap: BaseMap<CorporateData, long>
    {
        public CorporateDataMap()
        {
            ///Configure Properties
            this.Property(e => e.CorporateId).HasMaxLength(250);

            ///Conifgure Relationships.

            //one way to owner
            this.HasRequired(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }
    }
}
