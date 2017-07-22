using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class UserMap: BaseMap<string, UserEntity, User>
    {
        public UserMap()
        {
            Property(e => e.UniqueId).HasMaxLength(450);
        }

        public override void CopyToEntity(User model, UserEntity entity, ModelConverter converter)
        {
            entity.CreatedOn = model.CreatedOn;
            entity.GUId = model.UId;
            entity.ModifiedOn = model.ModifiedOn;
            entity.Status = model.Status;
            entity.UniqueId = model.UniqueId;
        }

        public override void CopyToModel(UserEntity entity, User model, ModelConverter converter)
        {
            model.CreatedOn = entity.CreatedOn;
            model.UId = entity.GUId;
            model.ModifiedOn = entity.ModifiedOn;
            model.Status = entity.Status;
            model.UniqueId = entity.UniqueId;
        }
    }
}
