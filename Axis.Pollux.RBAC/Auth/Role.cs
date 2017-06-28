using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Auth
{
    public class Role: PolluxModel<string>
    {
        private static readonly string AttributeName = "Role";

        public Role()
        { }

        public virtual string RoleName
        {
            get { return UniqueId; }
            set { UniqueId = value; }
        }

        public virtual string Name
        {
            get { return AttributeName; }
            set { }
        }

        public object Value
        {
            get { return RoleName; }
            set { RoleName = value?.ToString(); }
        }
    }
}
