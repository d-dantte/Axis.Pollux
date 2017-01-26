using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.RBAC.Auth
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ResourceAttribute: Attribute
    {
        private string[] _resourceNames;

        public IEnumerable<string> Resources => _resourceNames.ToList();

        public ResourceAttribute(params string[] resources)
        {
            _resourceNames = resources ?? new string[0];
        }
    }
}
