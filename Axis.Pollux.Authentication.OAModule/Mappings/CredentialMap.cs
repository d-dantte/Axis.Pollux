using Axis.Jupiter.Europa;
using Axis.Pollux.Authentication.OAModule.Entities;
using Axis.Pollux.Identity.OAModule.Mappings;
using static Axis.Luna.Extensions.ObjectExtensions;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Authentication.Models;

namespace Axis.Pollux.Authentication.OAModule.Mappings
{
    public class CredentialMap : BaseMap<long, CredentialEntity, Credential>
    {
        public CredentialMap()
        {
            ///Conifgure Relationships.
            this.HasRequired(e => e.Owner) //one way to owner
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }

        public override void CopyToEntity(Credential model, CredentialEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.ExpiresOn = model.ExpiresOn;
            entity.Metadata = converter.ToEntity(model.Metadata).Cast<CredentialMetadataEntity>();
            entity.ModifiedOn = model.ModifiedOn;
            if (model.Owner != null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<Identity.OAModule.Entities.UserEntity>();
                entity.OwnerId = model.Owner.UniqueId;
            }
            entity.SecuredHash = model.SecuredHash;
            entity.Tags = model.Tags;
            entity.UniqueId = model.UniqueId;
            entity.Value = model.Value;
        }

        public override void CopyToModel(CredentialEntity entity, Credential model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.ExpiresOn = entity.ExpiresOn;
            model.Metadata = converter.ToModel<CredentialMetadata>(entity.Metadata);
            model.ModifiedOn = entity.ModifiedOn;

            if (entity.Owner != null)
                model.Owner = converter.ToModel<User>(entity.Owner);
            else if (!string.IsNullOrWhiteSpace(entity.OwnerId))
                model.Owner = new User { UniqueId = entity.OwnerId };

            model.SecuredHash = entity.SecuredHash;
            model.Tags = entity.Tags;
            model.UniqueId = entity.UniqueId;
            model.Value = entity.Value;
        }
    }


    public class CredentialMetadataMap : Jupiter.Europa.Mappings.BaseComplexMapConfig<CredentialMetadata, CredentialMetadataEntity>
    {
        public CredentialMetadataMap()
        {
            ///Conifgure properties
            this.Property(c => c.Access)
                .HasColumnName($"Meta{nameof(CredentialMetadata.Access)}");

            this.Property(c => c.Name)
                .HasColumnName($"Meta{nameof(CredentialMetadata.Name)}");
        }

        public override void CopyToEntity(CredentialMetadata model, CredentialMetadataEntity entity, ModelConverter converter)
        {
            entity.Access = model.Access;
            entity.Name = model.Name;
        }

        public override void CopyToModel(CredentialMetadataEntity entity, CredentialMetadata model, ModelConverter converter)
        {
            model.Access = entity.Access;
            model.Name = entity.Name;
        }
    }

}
