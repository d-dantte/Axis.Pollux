using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Auth
{
    public class Role: PolluxEntity<string>
    {
        private static readonly string AttributeName = "Role";

        public Role()
        { }

        public virtual string RoleName
        {
            get { return EntityId; }
            set { EntityId = value; }
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
