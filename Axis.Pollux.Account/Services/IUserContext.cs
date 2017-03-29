using Axis.Luna;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Account.Services
{
    public interface IUserContext
    {
        Operation<string> CurrentUserId();
        Operation<UserLogon> CurrentUserLogon();

        Operation<IDataItem> UserAttribute(string name);
    }
}
