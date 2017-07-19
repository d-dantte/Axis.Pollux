using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;
using Axis.Pollux.RoleAuth.OAModule.Entities;
using Axis.Pollux.RoleAuth.Models;
using Axis.Luna.Extensions;

namespace Axis.Pollux.RoleAuth.OAModule.Mappings
{
    public class RolePermissionMap : BaseMap<long, RolePermissionEntity, RolePermission>
    {
        public RolePermissionMap()
        {
            this.Property(e => e.UUID)
                .IsIndex("PermissionGuid", true);

            this.HasRequired(_t => _t.Role)
                .WithMany()
                .HasForeignKey(_t => _t.RoleName);
        }

        public override void CopyToEntity(RolePermission model, RolePermissionEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.Effect = model.Effect;
            entity.Resource = model.Resource;
            entity.Label = model.Label;
            entity.ModifiedOn = model.ModifiedOn;
            entity.UniqueId = model.UniqueId;
            entity.UUID = model.UUID;

            if(model.Role != null)
            {
                entity.Role = converter.ToEntity(model.Role).Cast<RoleEntity>();
                entity.RoleName = model.Role.RoleName;
            }
        }

        public override void CopyToModel(RolePermissionEntity entity, RolePermission model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.Effect = entity.Effect;
            model.Resource = entity.Resource;
            model.Label = entity.Label;
            model.ModifiedOn = entity.ModifiedOn;
            model.UniqueId = entity.UniqueId;
            model.UUID = entity.UUID;

            if (entity.Role != null)
                model.Role = converter.ToModel<Role>(entity.Role);
            else if (!string.IsNullOrWhiteSpace(entity.RoleName))
                model.Role = new Role { RoleName = entity.RoleName };
        }
    }
}
