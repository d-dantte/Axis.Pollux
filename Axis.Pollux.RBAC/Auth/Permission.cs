using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Auth
{
    public class Permission: PolluxModel<long>
    {
        public string ResourceSelector
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public Role Role
        {
            get { return get<Role>(); }
            set { set(ref value); }
        }
        public string RoleId
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public Effect Effect
        {
            get { return get<Effect>(); }
            set { set(ref value); }
        }        
    }
}
