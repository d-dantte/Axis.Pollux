using System;
using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class UserDataMap: BaseMap<long, UserDataEntity, UserData>
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

            this.Property(e => e.Label)
                .HasMaxLength(400)
                .IsIndex("UserDataLabel");
        }

        public override void CopyToEntity(UserData model, UserDataEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.Data = model.Data;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Name = model.Name;
            entity.Type = model.Type;
            entity.Label = model.Label;
            entity.Status = model.Status;
            entity.UniqueId = model.UniqueId;

            if(model.Owner!=null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<UserEntity>();
                entity.OwnerId = model.Owner.UniqueId;
            }
        }

        public override void CopyToModel(UserDataEntity entity, UserData model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.Data = entity.Data;
            model.ModifiedOn = entity.ModifiedOn;
            model.Name = entity.Name;
            model.Type = entity.Type;
            model.Label = entity.Label;
            model.Status = entity.Status;
            model.UniqueId = entity.UniqueId;

            if (entity.Owner != null)
                model.Owner = converter.ToModel<User>(entity.Owner);
            else if (!string.IsNullOrWhiteSpace(entity.OwnerId))
                model.Owner = new User { UniqueId = entity.OwnerId };
        }
    }
}
