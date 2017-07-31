using Axis.Jupiter.Europa;
using Axis.Jupiter.Europa.Mappings;
using Axis.Pollux.Account.Models;
using Axis.Pollux.Account.OAModule.Entities;
using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.AccountManagement.OAModule.Mappings
{
    public class UserLogonMap : BaseMap<long, UserLogonEntity, UserLogon>
    {
        public UserLogonMap()
        {
            this.Property(e => e.Locale).HasMaxLength(20);
            this.Property(e => e.SecurityToken).HasMaxLength(250);
            this.Property(e => e.IPAddress).HasMaxLength(250);

            this.HasRequired(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }

        public override void CopyToEntity(UserLogon model, UserLogonEntity entity, ModelConverter converter)
        {
            entity.Client = converter.ToEntity(model.Client).Cast<UserAgentEntity>(); //<-- being a complex type, it should never be null
            entity.CreatedOn = model.CreatedOn;
            entity.Invalidated = model.Invalidated;
            entity.Locale = model.Locale;
            entity.Location = model.Location;
            entity.ModifiedOn = model.ModifiedOn;
            entity.SecurityToken = model.SecurityToken;
            entity.TimeZoneOffset = model.TimeZoneOffset;
            entity.UniqueId = model.UniqueId;
            entity.IPAddress = model.IPAddress;
            
            if(model.User != null)
            {
                entity.User = converter.ToEntity(model.User).Cast<UserEntity>();
                entity.UserId = model.User.UniqueId;
            }
        }

        public override void CopyToModel(UserLogonEntity entity, UserLogon model, ModelConverter converter)
        {
            model.Client = converter.ToModel<UserAgent>(entity.Client); //<-- being a complex type, it should never be null
            model.CreatedOn = entity.CreatedOn;
            model.Invalidated = entity.Invalidated;
            model.Locale = entity.Locale;
            model.Location = entity.Location;
            model.ModifiedOn = entity.ModifiedOn;
            model.SecurityToken = entity.SecurityToken;
            model.TimeZoneOffset = entity.TimeZoneOffset;
            model.UniqueId = entity.UniqueId;
            model.IPAddress = entity.IPAddress;

            if (entity.User != null)
                model.User = converter.ToModel<User>(entity.User);

            else if (!string.IsNullOrWhiteSpace(entity.UserId))
                model.User = new User { UniqueId = entity.UserId };
        }
    }


    public class UserAgentMap : BaseComplexMapConfig<UserAgent, UserAgentEntity>
    {
        public UserAgentMap()
        {
            Property(e => e.Browser).HasMaxLength(300);
            Property(e => e.BrowserVersion).HasMaxLength(20);
            Property(e => e.OS).HasMaxLength(300);
            Property(e => e.OSVersion).HasMaxLength(20);
            Property(e => e.Device).HasMaxLength(300);
        }

        public override void CopyToEntity(UserAgent model, UserAgentEntity entity, ModelConverter converter)
        {
            entity.Browser = model.Browser;
            entity.BrowserVersion = model.BrowserVersion;
            entity.Device = model.Device;
            entity.OS = model.OS;
            entity.OSVersion = model.OSVersion;
        }

        public override void CopyToModel(UserAgentEntity entity, UserAgent model, ModelConverter converter)
        {
            model.Browser = entity.Browser;
            model.BrowserVersion = entity.BrowserVersion;
            model.Device = entity.Device;
            model.OS = entity.OS;
            model.OSVersion = entity.OSVersion;
        }
    }
}
