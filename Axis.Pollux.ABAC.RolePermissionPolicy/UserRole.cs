using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.RolePermissionPolicy
{
    public class UserRole: PolluxEntity<long>
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public string RoleName { get; set; }
    }
}
