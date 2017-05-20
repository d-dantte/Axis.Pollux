using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Auth
{
    public class UserRole: PolluxEntity<long>
    {
        public string UserId
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public string RoleName
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
    }
}
