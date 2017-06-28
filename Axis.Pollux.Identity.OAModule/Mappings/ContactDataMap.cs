using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class ContactDataMap: BaseMap<long, ContactDataEntity, ContactData>
    {
        public ContactDataMap()
        {
            ///Configure Properties
            this.Property(e => e.AlternateEmail).HasMaxLength(250);
            this.Property(e => e.AlternatePhone).HasMaxLength(50);
            this.Property(e => e.Email).HasMaxLength(250);
            this.Property(e => e.Phone).HasMaxLength(50);


            ///Conifgure Relationships.            
            this.HasRequired(e => e.Owner)//one way to owner
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }

        public override void CopyToEntity(ContactData model, ContactDataEntity entity, ModelConverter converter)
        {
            entity.AlternateEmail = model.AlternateEmail;
            entity.AlternatePhone = model.AlternatePhone;
            entity.CreatedOn = model.CreatedOn;
            entity.Email = model.Email;
            entity.EmailConfirmed = model.EmailConfirmed;
            entity.ModifiedOn = model.ModifiedOn;
            if (model.Owner != null)
            {
                entity.Owner = converter.ToEntity(model.Owner).Cast<UserEntity>();
                entity.OwnerId = model.Owner.UniqueId;
            }
            entity.Phone = model.Phone;
            entity.PhoneConfirmed = model.PhoneConfirmed;
            entity.Status = model.Status;
            entity.UniqueId = model.UniqueId;
        }

        public override void CopyToModel(ContactDataEntity entity, ContactData model, ModelConverter converter)
        {
            model.AlternateEmail = entity.AlternateEmail;
            model.AlternatePhone = entity.AlternatePhone;
            model.CreatedOn = entity.CreatedOn;
            model.Email = entity.Email;
            model.EmailConfirmed = entity.EmailConfirmed;
            model.ModifiedOn = entity.ModifiedOn;
            if (model.Owner != null)
                model.Owner = converter.ToModel<User>(model.Owner);
            else if (!string.IsNullOrWhiteSpace(entity.OwnerId))
                model.Owner = new User { UniqueId = entity.Owner.UniqueId };

            model.Phone = entity.Phone;
            model.PhoneConfirmed = entity.PhoneConfirmed;
            model.Status = entity.Status;
            model.UniqueId = entity.UniqueId;
        }
    }
}
