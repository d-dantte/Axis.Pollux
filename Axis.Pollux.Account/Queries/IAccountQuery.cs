using Axis.Luna;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Account.Queries
{
    public interface IAccountQuery
    {
        User GetUser(string userId);
        SequencePage<UserLogon> GetValidUserLogons(string userId, int pageSize = 0, int pageIndex = 0, bool includeCount = false);
        ContextVerification GetLatestVerification(string userId, string context);
    }
}
