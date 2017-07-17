using Axis.Pollux.ABAC.DAS.Models;
using Axis.Pollux.ABAC.DAS.OAModule.Entities;
using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.ABAC.DAS.OAModule.Mappings
{
    public class RolePermissionMap : BaseMap<long, RolePermissionEntity, RolePermission>
    {
        public RolePermissionMap()
        {
            this.Property(e => e.RoleName)
                .HasMaxLength(450)
                .IsIndex("PermissionRoleName");

            this.Property(e => e.PermissionGuid)
                .IsIndex("PermissionGuidIndex", true);
        }

        public override void CopyToEntity(RolePermission model, RolePermissionEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.Effect = model.Effect;
            entity.IntentDescriptor = model.IntentDescriptor;
            entity.PolicyCode = model.Label;
            entity.ModifiedOn = model.ModifiedOn;
            entity.RoleName = model.RoleName;
            entity.UniqueId = model.UniqueId;
            entity.PermissionGuid = model.PermissionGuid;
        }

        public override void CopyToModel(RolePermissionEntity entity, RolePermission model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.Effect = entity.Effect;
            model.IntentDescriptor = entity.IntentDescriptor;
            model.Label = entity.PolicyCode;
            model.ModifiedOn = entity.ModifiedOn;
            model.RoleName = entity.RoleName;
            model.UniqueId = entity.UniqueId;
            model.PermissionGuid = entity.PermissionGuid;
        }
    }
}
