using Axis.Pollux.Identity.OAModule.Mappings;
using System.Collections.Generic;
using Axis.Jupiter.Europa;
using Axis.Luna.Extensions;
using Newtonsoft.Json;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Notification.Email.EF.Entities;

namespace Axis.Pollux.Notification.Email.EF.Mappings
{
    public class NotificationMapping : BaseMap<long, EmailNotificationEntity, Models.Notification>
    {
        public NotificationMapping()
        {
            this.HasRequired(m => m.Target)
                .WithMany()
                .HasForeignKey(m => m.TargetId);

            this.Property(m => m.Origin)
                .HasMaxLength(450);

            this.Property(m => m.UUId)
                .IsIndex("UUID_Index", true);

            this.Property(m => m.Data)
                .HasColumnType("ntext");
        }

        public override void CopyToEntity(Models.Notification model, EmailNotificationEntity entity, ModelConverter converter)
        {
            entity.Channels = model.Channels?.JoinUsing(",");
            entity.CreatedOn = model.CreatedOn;
            entity.Data = JsonConvert.SerializeObject(model.Data, Constants.NotificationDataSerializationSettings);
            entity.ModifiedOn = model.ModifiedOn;
            entity.Origin = model.Origin;
            entity.Status = model.Status;

            if (model.Target != null)
            {
                entity.Target = converter.ToEntity<User, UserEntity>(model.Target);
                entity.TargetId = model.Target.UniqueId;
            }

            entity.UniqueId = model.UniqueId;
            entity.UUId = model.UUId;
        }

        public override void CopyToModel(EmailNotificationEntity entity, Models.Notification model, ModelConverter converter)
        {
            model.Channels = entity.Channels?.Split(',') ?? new string[0];
            model.CreatedOn = entity.CreatedOn;
            model.Data = JsonConvert.DeserializeObject<Dictionary<string, object>>(entity.Data, Constants.NotificationDataSerializationSettings);
            model.ModifiedOn = entity.ModifiedOn;
            model.Origin = entity.Origin;
            model.Status = entity.Status;

            if (entity.Target != null)
                model.Target = converter.ToModel<UserEntity, User>(entity.Target);

            else if (!string.IsNullOrWhiteSpace(entity.TargetId))
                model.Target = new User { UniqueId = entity.TargetId };

            model.UniqueId = entity.UniqueId;
            model.UUId = entity.UUId;
        }
    }
}
