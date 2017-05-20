using Axis.Luna;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.AccountManagement.Queries
{
    public interface IAccountQuery
    {
        User GetUser(string userId);
        long UserCount();
        SequencePage<UserLogon> GetValidUserLogons(string userId, int pageSize = 0, int pageIndex = 0, bool includeCount = false);
        ContextVerification GetLatestVerification(string userId, string context);
        ContextVerification GetContextVerification(string userId, string context, string token);
    }
}
