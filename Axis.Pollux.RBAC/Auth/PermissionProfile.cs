using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;

namespace Axis.Pollux.RBAC.Auth
{
    public class PermissionProfile
    {
        private List<string> _resourceNames = new List<string>();
        public IEnumerable<string> Resources
        {
            get { return _resourceNames; }
            set
            {
                _resourceNames.Clear();
                if (value != null) _resourceNames.AddRange(value);
            }
        }

        public User Principal { get; set; }
    }
}
