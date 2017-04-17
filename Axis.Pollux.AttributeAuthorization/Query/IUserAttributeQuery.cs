using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.Query
{
    public interface IUserAttributeQuery
    {
        IEnumerable<UserData> GetUserAttributes(User user);
        UserData GetUserAttribute(User user, string attributeName);
    }
}
