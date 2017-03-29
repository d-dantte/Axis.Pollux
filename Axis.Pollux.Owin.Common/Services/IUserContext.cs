using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Owin.Common.Services
{
    public interface IUserContext
    {
        User CurrentUser();
        IEnumerable<string> CurrentUserRoles();
        //UserLogon CurrentUserLogon();
    }
}
