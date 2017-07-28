using Axis.Luna;
using Axis.Pollux.Account.Models;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.AccountManagement.Queries
{
    public interface IAccountQuery
    {
        User GetUser(string userId);
        long UserCount();
        SequencePage<UserLogon> GetValidUserLogons(string userId, PageParams pageParams = null);
        ContextVerification GetLatestVerification(string userId, string context);
        ContextVerification GetContextVerification(string userId, string context, string token);
    }
}
