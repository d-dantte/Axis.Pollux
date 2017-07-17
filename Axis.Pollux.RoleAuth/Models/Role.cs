using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RoleAuth.Models
{
    public class Role: PolluxModel<string>
    {
        public string RoleName
        {
            get { return UniqueId; }
            set { UniqueId = value; }
        }

        public RoleStatus Status { get; set; }
    }

    public enum RoleStatus
    {
        Enabled,
        Disabled
    }
}
