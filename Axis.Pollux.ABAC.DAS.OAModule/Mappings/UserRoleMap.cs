using Axis.Pollux.ABAC.DAS.Models;
using Axis.Pollux.ABAC.DAS.OAModule.Entities;
using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.DAS.OAModule.Mappings
{
    public class UserRoleMap : BaseMap<long, UserRoleEntity, UserRole>
    {
        public UserRoleMap()
        {
            this.Property(e => e.RoleName)
                .HasMaxLength(450)
                .IsIndex("UserRoleName");
        }

        public override void CopyToEntity(UserRole model, UserRoleEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.ModifiedOn = model.ModifiedOn;
            entity.RoleName = model.RoleName;
            entity.UniqueId = model.UniqueId;
            
            if(model.User != null)
            {
                entity.User = converter.ToEntity(model).Cast<UserEntity>();
                entity.UserId = model.User.UserId;
            }
        }

        public override void CopyToModel(UserRoleEntity entity, UserRole model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.ModifiedOn = entity.ModifiedOn;
            model.RoleName = entity.RoleName;
            model.UniqueId = entity.UniqueId;

            if (entity.User != null)
                model.User = converter.ToModel<User>(entity.User);
            else if (!string.IsNullOrWhiteSpace(entity.UserId))
                model.User = new User { UserId = entity.UserId };
        }
    }
}
