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
        }

        public override void CopyToEntity(UserData model, UserDataEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.Data = model.Data;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Name = model.Name;
            entity.Type = model.Type;
            entity.UniqueId = model.UniqueId;

            if(model.Owner!=null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<UserEntity>();
                entity.OwnerId = model.Owner.UniqueId;
            }
        }

        public override void CopyToModel(UserDataEntity entity, UserData model, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.Data = model.Data;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Name = model.Name;
            entity.Type = model.Type;
            entity.UniqueId = model.UniqueId;

            if (model.Owner != null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<UserEntity>();
                entity.OwnerId = model.Owner.UniqueId;
            }
        }
    }
}
