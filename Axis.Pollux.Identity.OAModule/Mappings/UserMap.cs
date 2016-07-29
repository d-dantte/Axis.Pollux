using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class UserMap: BaseMap<User, string>
    {
        public UserMap()
        {
            ///Configure Ignored properties
            this.Ignore(e => e.UserId);

            Property(e => e.EntityId).HasMaxLength(250);
        }
    }
}
