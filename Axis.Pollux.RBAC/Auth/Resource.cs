using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Auth
{
    public class Resource: PolluxEntity<long>
    {
        public string Name
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public string Description
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
    }
}
