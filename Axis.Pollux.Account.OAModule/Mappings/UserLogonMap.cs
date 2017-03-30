using Axis.Jupiter.Europa.Mappings;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Identity.OAModule.Mappings;

namespace Axis.Pollux.Account.OAModule.Mappings
{
    public class UserLogonMap : BaseMap<UserLogon, long>
    {
        public UserLogonMap()
        {
            this.Property(e => e.Locale).HasMaxLength(20);
            this.Property(e => e.SecurityToken).HasMaxLength(100);

            this.HasRequired(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }

    public class UserAgentMap : BaseComplexMap<UserAgent>
    {
        public UserAgentMap()
        {
            this.Property(e => e.Browser).HasMaxLength(300);
            this.Property(e => e.BrowserVersion).HasMaxLength(20);
            this.Property(e => e.OS).HasMaxLength(300);
            this.Property(e => e.OSVersion).HasMaxLength(20);
            this.Property(e => e.Device).HasMaxLength(300);
        }
    }
}
