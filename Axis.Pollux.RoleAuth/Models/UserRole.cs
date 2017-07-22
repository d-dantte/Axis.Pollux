using Axis.Pollux.Common;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RoleAuth.Models
{
    public class UserRole: PolluxModel<long>
    {
        public User User { get; set; }

        public Role Role { get; set; }
    }
}
