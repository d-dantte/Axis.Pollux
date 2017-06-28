using System;
using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class CorporateDataMap: BaseMap<long, CorporateDataEntity, CorporateData>
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

        public override void CopyToEntity(CorporateData model, CorporateDataEntity entity, ModelConverter converter)
        {
            entity.CorporateId = model.CorporateId;
            entity.CorporateName = model.CorporateName;
            entity.CreatedOn = model.CreatedOn;
            entity.Description = model.Description;
            entity.IncorporationDate = model.IncorporationDate;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Status = model.Status;
            entity.UniqueId = model.UniqueId;

            if(model.Owner != null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<UserEntity>();
                entity.OwnerId = model.Owner.UniqueId;
            }
        }

        public override void CopyToModel(CorporateDataEntity entity, CorporateData model, ModelConverter converter)
        {
            model.CorporateId = entity.CorporateId;
            model.CorporateName = entity.CorporateName;
            model.CreatedOn = entity.CreatedOn;
            model.Description = entity.Description;
            model.IncorporationDate = entity.IncorporationDate;
            model.ModifiedOn = entity.ModifiedOn;
            model.Status = entity.Status;
            model.UniqueId = entity.UniqueId;

            if (entity.Owner != null)
                model.Owner = converter.ToModel<User>(entity.Owner);
            else if (!string.IsNullOrWhiteSpace(entity.OwnerId))
                model.Owner = new User { UniqueId = entity.OwnerId };
        }
    }
}
