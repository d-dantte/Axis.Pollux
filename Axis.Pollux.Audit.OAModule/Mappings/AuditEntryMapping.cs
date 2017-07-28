using Axis.Pollux.Audit.Models;
using Axis.Pollux.Audit.OAModule.Entities;
using Axis.Pollux.Identity.OAModule.Mappings;
using System;
using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Audit.OAModule.Mappings
{
    public class AuditEntryMapping : BaseMap<long, AuditEntryEntity, AuditEntry>
    {
        public AuditEntryMapping()
        {
            this.HasRequired(_ => _.User)
                .WithMany()
                .HasForeignKey(_ => _.UserId);

            this.Property(_ => _.ContextData)
                .HasColumnType("ntext");
        }

        public override void CopyToEntity(AuditEntry model, AuditEntryEntity entity, ModelConverter converter)
        {
            entity.Action = model.Action;
            entity.ContextData = model.ContextData;
            entity.CreatedOn = model.CreatedOn;
            entity.ModifiedOn = model.ModifiedOn;
            entity.UniqueId = model.UniqueId;
            
            if(model.User != null)
            {
                entity.User = converter.ToEntity(model.User).Cast<UserEntity>();
                entity.UserId = model.User.UserId;
            }
        }

        public override void CopyToModel(AuditEntryEntity entity, AuditEntry model, ModelConverter converter)
        {
            model.Action = entity.Action;
            model.ContextData = entity.ContextData;
            model.CreatedOn = entity.CreatedOn;
            model.ModifiedOn = entity.ModifiedOn;
            model.UniqueId = entity.UniqueId;

            if (entity.User != null)
                model.User = converter.ToModel<User>(entity.User);
            else if(!string.IsNullOrWhiteSpace(entity.UserId))
                model.User = new User { UserId = entity.UserId };
        }
    }
}
