using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.RBAC.Auth
{
    public class Resource: PolluxModel<long>
    {
        public string Path
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
