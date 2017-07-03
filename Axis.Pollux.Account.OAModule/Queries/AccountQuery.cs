using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;
using Axis.Pollux.AccountManagement.Queries;
using Axis.Pollux.Account.Models;
using Axis.Jupiter.Europa;
using Axis.Pollux.Account.OAModule.Entities;
using Axis.Pollux.Identity.OAModule.Entities;

namespace Axis.Pollux.AccountManagement.OAModule.Queries
{
    public class AccountQuery: IAccountQuery
    {
        private DataStore _europa = null;

        public AccountQuery(DataStore context)
        {
            ThrowNullArguments(() => context);

            _europa = context;
        }

        public ContextVerification GetContextVerification(string userId, string context, string token)
        => _europa.Query<ContextVerificationEntity>(_cv => _cv.Target)
                  .Where(_cv => _cv.TargetId == userId)
                  .Where(_cv => _cv.Context == context)
                  .Where(_cv => _cv.VerificationToken == token)
                  .FirstOrDefault()
                  .Pipe(new ModelConverter(_europa).ToModel<ContextVerification>);

        public ContextVerification GetLatestVerification(string userId, string context)
        => _europa.Query<ContextVerificationEntity>(_cv => _cv.Target)
                  .Where(_cv => _cv.TargetId == userId)
                  .Where(_cv => _cv.Context == context)
                  .OrderByDescending(_cv => _cv.CreatedOn)
                  .FirstOrDefault()
                  .Pipe(new ModelConverter(_europa).ToModel<ContextVerification>);

        public User GetUser(string userId)
        => _europa.Query<UserEntity>()
                  .Where(_u => _u.UniqueId == userId)
                  .FirstOrDefault()
                  .Pipe(new ModelConverter(_europa).ToModel<User>);

        public SequencePage<UserLogon> GetValidUserLogons(string userId, int pageSize = -1, int pageIndex = 0, bool includeCount = false)
        => _europa.Query<UserLogonEntity>()
                  .Where(_u => _u.UserId == userId)
                  .Where(_u => !_u.Invalidated)
                  .OrderByDescending(_u => _u.ModifiedOn)
                  .Pipe(_q =>
                  {
                      var converter = new ModelConverter(_europa);
                      var d = _q
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .AsEnumerable() //<-- pull from DB
                        .Select(converter.ToModel<UserLogon>)
                        .ToArray();

                      var count = includeCount ? _q.Count() : d.Length;
                      return new SequencePage<UserLogon>(d, count, pageSize, pageIndex);
                  });

        public long UserCount()
        => _europa.Query<UserEntity>().LongCount();
    }
}
