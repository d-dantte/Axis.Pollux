using Axis.Luna;
using Axis.Pollux.Account.Models;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Common.Models;
using System.Collections.Generic;

namespace Axis.Pollux.AccountManagement.Queries
{
    public interface IAccountQuery
    {
        User GetUser(string userId);
        long UserCount();
        SequencePage<UserLogon> GetValidUserLogons(string userId, PageParams pageParams = null);
        ContextVerification GetLatestVerification(string userId, string context);
        ContextVerification GetContextVerification(string userId, string context, string token);

        UserLogon GetUserLogonWithToken(string userId, string token);
        IEnumerable<UserLogon> GetUserLogons(string userId, string ipaddress = null, string location = null, string locale = null, string device = null);
    }
}
