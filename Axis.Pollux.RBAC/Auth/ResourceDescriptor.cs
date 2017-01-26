using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.RBAC.Auth
{
    public class ResourceDescriptor: PolluxEntity<long>
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

        public bool Match(string resourcePath)
        {
            throw new NotImplementedException();
        }
    }
}
