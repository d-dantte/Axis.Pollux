using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Pollux.RoleAuth.Models;
using Axis.Pollux.RoleAuth.OAModule.Entities;
using System;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.RoleAuth.OAModule.Mappings
{
    public class RoleMap : BaseMap<string, RoleEntity, Role>
    {
        public RoleMap()
        {

        }

        public override void CopyToEntity(Role model, RoleEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Status = model.Status;
            entity.UniqueId = model.UniqueId;
        }

        public override void CopyToModel(RoleEntity entity, Role model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.ModifiedOn = entity.ModifiedOn;
            model.Status = entity.Status;
            model.UniqueId = entity.UniqueId;
        }
    }
}
