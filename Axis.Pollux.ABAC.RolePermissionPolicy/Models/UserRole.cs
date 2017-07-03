using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.RolePermissionPolicy
{
    public class UserRole: PolluxModel<long>
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string RoleName { get; set; }
    }
}
