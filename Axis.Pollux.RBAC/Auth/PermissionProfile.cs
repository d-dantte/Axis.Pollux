using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;

namespace Axis.Pollux.RBAC.Auth
{
    /// <summary>
    /// An object used to express to the authorization service, the intent to verify that the user has the right to access
    /// resources designated by the resource-paths provided in the <c>ResourcePaths</c> list. The <c>CombinationMethod</c>
    /// enumerable specifies how the permissions should be combined to give a final result
    /// </summary>
    public class PermissionProfile
    {
        private List<string> _resourcePaths = new List<string>();
        public IEnumerable<string> ResourcePaths
        {
            get { return _resourcePaths; }
            set
            {
                _resourcePaths.Clear();
                if (value != null) _resourcePaths.AddRange(value);
            }
        }

        public CombinationMethod CombinationMethod { get; set; }
        public User Principal { get; set; }
    }

    public enum CombinationMethod
    {
        All,
        Any
    }
}
