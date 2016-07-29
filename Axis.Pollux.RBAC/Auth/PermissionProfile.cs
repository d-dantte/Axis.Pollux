using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;

namespace Axis.Pollux.RBAC.Auth
{
    public class PermissionProfile
    {
        private List<string> _grant = new List<string>();
        public IEnumerable<string> Grant
        {
            get { return _grant; }
            set
            {
                _grant.Clear();
                if (value != null) _grant.AddRange(value);
            }
        }

        private List<string> _deny = new List<string>();
        public IEnumerable<string> Deny
        {
            get { return _deny; }
            set
            {
                _deny.Clear();
                if (value != null) _deny.AddRange(value);
            }
        }

        public User Principal { get; set; }
    }
}
