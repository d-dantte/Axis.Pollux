using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.RoleAuth.OAModule.Entities;
using Axis.Pollux.RoleAuth.Models;

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
            entity.UniqueId = model.UniqueId;

            if (model.User != null)
            {
                entity.User = converter.ToEntity(model.User).Cast<UserEntity>();
                entity.UserId = model.User.UserId;
            }

            if (model.Role != null)
            {
                entity.Role = converter.ToEntity(model.Role).Cast<RoleEntity>();
                entity.RoleName = model.Role.RoleName;
            }
        }

        public override void CopyToModel(UserRoleEntity entity, UserRole model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.ModifiedOn = entity.ModifiedOn;
            model.UniqueId = entity.UniqueId;

            if (entity.User != null)
                model.User = converter.ToModel<User>(entity.User);
            else if (!string.IsNullOrWhiteSpace(entity.UserId))
                model.User = new User { UserId = entity.UserId };

            if (entity.Role != null)
                model.Role = converter.ToModel<Role>(entity.Role);
            else if (!string.IsNullOrWhiteSpace(entity.RoleName))
                model.Role = new Role { RoleName = entity.RoleName };
        }
    }
}
