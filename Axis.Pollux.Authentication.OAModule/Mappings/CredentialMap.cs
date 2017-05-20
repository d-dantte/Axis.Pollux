using Axis.Jupiter.Europa.Mappings;
using Axis.Pollux.Identity.OAModule.Mappings;

namespace Axis.Pollux.Authentication.OAModule.Mappings
{
    public class CredentialMap : BaseMap<Credential, long>
    {
        public CredentialMap()
        {
            ///Conifgure Relationships.
            this.HasRequired(e => e.Owner) //one way to owner
                .WithMany()
                .HasForeignKey(e => e.OwnerId);

            this.Ignore(c => c.Expires);
        }
    }
    public class CredentialMetadataMap : BaseComplexMap<CredentialMetadata>
    {
        public CredentialMetadataMap()
        {
            ///Conifgure properties
            this.Property(c => c.Access)
                .HasColumnName($"Meta{nameof(CredentialMetadata.Access)}");

            this.Property(c => c.Name)
                .HasColumnName($"Meta{nameof(CredentialMetadata.Name)}");
        }
    }

}
