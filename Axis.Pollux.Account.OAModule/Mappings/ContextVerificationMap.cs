using Axis.Jupiter.Europa;
using Axis.Pollux.Account.Models;
using Axis.Pollux.Account.OAModule.Entities;
using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.AccountManagement.OAModule.Mappings
{
    public class ContextVerificationMap: BaseMap<long, ContextVerificationEntity, ContextVerification>
    {
        public ContextVerificationMap()
        {
            ///Conifgure Relationships.
            this.HasRequired(e => e.Target) //one way to owner
                .WithMany()
                .HasForeignKey(e => e.TargetId);

            this.Property(e => e.VerificationToken)
                .HasMaxLength(100)
                .IsIndex("VerificationContextName");
        }

        public override void CopyToEntity(ContextVerification model, ContextVerificationEntity entity, ModelConverter converter)
        {
            entity.Context = model.Context;
            entity.CreatedOn = model.CreatedOn;
            entity.ExpiresOn = model.ExpiresOn;
            entity.ModifiedOn = model.ModifiedOn;
            entity.UniqueId = model.UniqueId;
            entity.VerificationToken = model.VerificationToken;
            entity.Verified = model.Verified;

            if(model.Target != null)
            {
                entity.Target = converter.ToEntity(model.Target).Cast<UserEntity>();
                entity.TargetId = model.Target.UniqueId;
            }
        }

        public override void CopyToModel(ContextVerificationEntity entity, ContextVerification model, ModelConverter converter)
        {
            model.Context = entity.Context;
            model.CreatedOn = entity.CreatedOn;
            model.ExpiresOn = entity.ExpiresOn;
            model.ModifiedOn = entity.ModifiedOn;
            model.UniqueId = entity.UniqueId;
            model.VerificationToken = entity.VerificationToken;
            model.Verified = entity.Verified;

            if (entity.Target != null)
                model.Target = converter.ToModel<User>(entity.Target);

            else if (!string.IsNullOrWhiteSpace(entity.TargetId))
                model.Target = new User { UniqueId = entity.TargetId };
        }
    }
}
