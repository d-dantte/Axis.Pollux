using Axis.Jupiter;
using System;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;
using Axis.Pollux.AccountManagement.Queries;
using Axis.Pollux.Account.Objects;

namespace Axis.Pollux.AccountManagement.OAModule.Queries
{
    public class AccountQuery: IAccountQuery
    {
        private IDataContext _europa = null;

        public AccountQuery(IDataContext context)
        {
            ThrowNullArguments(() => context);

            _europa = context;
        }

        public ContextVerification GetContextVerification(string userId, string context, string token)
        => _europa.Store<ContextVerification>()
                  .QueryWith(_cv => _cv.Target)
                  .Where(_cv => _cv.TargetId == userId)
                  .Where(_cv => _cv.Context == context)
                  .Where(_cv => _cv.VerificationToken == token)
                  .FirstOrDefault();

        public ContextVerification GetLatestVerification(string userId, string context)
        => _europa.Store<ContextVerification>()
                  .QueryWith(_cv => _cv.Target)
                  .Where(_cv => _cv.TargetId == userId)
                  .Where(_cv => _cv.Context == context)
                  .OrderByDescending(_cv => _cv.CreatedOn)
                  .FirstOrDefault();

        public User GetUser(string userId)
        => _europa.Store<User>().Query
                  .Where(_u => _u.UniqueId == userId)
                  .FirstOrDefault();

        public SequencePage<UserLogon> GetValidUserLogons(string userId, int pageSize = 0, int pageIndex = 0, bool includeCount = false)
        => _europa.Store<UserLogon>().Query
                  .Where(_u => _u.UserId == userId)
                  .Where(_u => !_u.Invalidated)
                  .OrderByDescending(_u => _u.ModifiedOn)
                  .Pipe(_q =>
                  {
                      pageIndex = Math.Abs(pageIndex);
                      var page = _q.Skip(Math.Abs(pageSize) * pageIndex).Take(pageIndex).ToArray(); //else give me the first (pageSize) items from the query
                      var count = includeCount ? _q.Count() : 0;

                      return new SequencePage<UserLogon>(page, count, pageSize, pageIndex);
                  });

        public long UserCount()
        => _europa.Store<User>().Query.Count();
    }
}
