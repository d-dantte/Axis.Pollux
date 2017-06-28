using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class BioDataMap: BaseMap<long, BioDataEntity, BioData>
    {
        public BioDataMap()
        {
            ///Configure Properties
            this.Property(e => e.FirstName).HasMaxLength(100);
            this.Property(e => e.LastName).HasMaxLength(100);
            this.Property(e => e.MiddleName).HasMaxLength(100);
            this.Property(e => e.Nationality).HasMaxLength(250);
            this.Property(e => e.StateOfOrigin).HasMaxLength(250);

            ///Conifgure Relationships.
            this.HasRequired(e => e.Owner) //one way to owner
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }

        public override void CopyToEntity(BioData model, BioDataEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.Dob = model.Dob;
            entity.FirstName = model.FirstName;
            entity.Gender = model.Gender;
            entity.LastName = model.LastName;
            entity.MiddleName = model.MiddleName;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Nationality = model.Nationality;
            if (model.Owner != null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<UserEntity>();
                entity.OwnerId = model.Owner.UniqueId;
            }
            entity.StateOfOrigin = model.StateOfOrigin;
            entity.UniqueId = model.UniqueId;
        }

        public override void CopyToModel(BioDataEntity entity, BioData model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.Dob = entity.Dob;
            model.FirstName = entity.FirstName;
            model.Gender = entity.Gender;
            model.LastName = entity.LastName;
            model.MiddleName = entity.MiddleName;
            model.ModifiedOn = entity.ModifiedOn;
            model.Nationality = entity.Nationality;
            if (entity.Owner != null)
                model.Owner = converter.ToModel<User>(entity.Owner);
            else if (!string.IsNullOrWhiteSpace(entity.OwnerId))
                model.Owner = new User { UniqueId = entity.OwnerId };

            model.StateOfOrigin = entity.StateOfOrigin;
            model.UniqueId = entity.UniqueId;
        }
    }
}
