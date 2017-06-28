using Axis.Luna.Utils;
using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;

namespace Axis.Pollux.Common.Services
{
    public interface IUserContext
    {
        User User();

        IEnumerable<IDataItem> UserAttributes();
        IDataItem UserAttribute(string name);
    }
}
