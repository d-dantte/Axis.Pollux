using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Auth
{
    public class Permission: PolluxEntity<long>
    {
        public Resource Resource
        {
            get { return get<Resource>(); }
            set { set(ref value); }
        }

        public Role Role
        {
            get { return get<Role>(); }
            set { set(ref value); }
        }

        public Effect Effect
        {
            get { return get<Effect>(); }
            set { set(ref value); }
        }        
    }
}
