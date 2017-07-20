using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class AddressDataMap: BaseMap<long, AddressDataEntity, AddressData>
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

        public override void CopyToEntity(AddressData model, AddressDataEntity entity, ModelConverter converter)
        {
            entity.City = model.City;
            entity.Country = model.Country;
            entity.CreatedOn = model.CreatedOn;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Status = model.Status;
            if (model.Owner != null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<UserEntity>();
                entity.OwnerId = entity.Owner.UniqueId;
            }
            entity.StateProvince = model.StateProvince;
            entity.Street = model.Street;
            entity.UniqueId = model.UniqueId;
        }

        public override void CopyToModel(AddressDataEntity entity, AddressData model, ModelConverter converter)
        {
            model.City = entity.City;
            model.Country = entity.Country;
            model.CreatedOn = entity.CreatedOn;
            model.ModifiedOn = entity.ModifiedOn;
            model.Status = entity.Status;
            if (entity.Owner != null)
                model.Owner = converter.ToModel<User>(entity.Owner).Cast<User>();
            else if (!string.IsNullOrWhiteSpace(entity.OwnerId))
                model.Owner = new User { UserId = entity.OwnerId };

            model.StateProvince = entity.StateProvince;
            model.Street = entity.Street;
            model.UniqueId = entity.UniqueId;
        }
    }
}
