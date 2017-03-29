using Axis.Jupiter;
using Axis.Pollux.Identity.Queries;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;
using System;

namespace Axis.Pollux.Identity.OAModule.Queries
{
    public class UserQuery: IUserQuery
    {
        private IDataContext _europa = null;

        public UserQuery(IDataContext context)
        {
            ThrowNullArguments(() => context);

            _europa = context;
        }

        public BioData GetBioData(string userId)
        => _europa.Store<BioData>().QueryWith(_bd => _bd.Owner)
                  .Where(_bd => _bd.OwnerId == userId)
                  .FirstOrDefault();

        public ContactData GetContactData(long id)
        => _europa.Store<ContactData>().QueryWith(_cd => _cd.Owner)
                  .Where(_cd => _cd.EntityId == id)
                  .FirstOrDefault();

        public SequencePage<ContactData> GetContactData(string userId, int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => _europa.Store<ContactData>().QueryWith(_cd => _cd.Owner)
                  .Where(_cd => _cd.OwnerId == userId)
                  .OrderBy(_cd => _cd.CreatedOn)
                  .Pipe(_q =>
                  {
                      var d = _q.Skip(pageSize * pageIndex).Take(pageSize).ToArray();
                      return new SequencePage<ContactData>(d, includeCount ? _q.Count() : d.Length, pageSize, pageIndex);
                  });

        public long GetUserCount()
        => _europa.Store<User>().Query.LongCount();

        public UserData GetUserData(string userId, string dataName)
        => _europa.Store<UserData>().QueryWith(_cd => _cd.Owner)
                  .Where(_cd => _cd.OwnerId == userId)
                  .Where(_cd => _cd.Name == dataName)
                  .FirstOrDefault();

        public SequencePage<UserData> GetUserData(string userId, int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => _europa.Store<UserData>().QueryWith(_cd => _cd.Owner)
                  .Where(_cd => _cd.OwnerId == userId)
                  .OrderBy(_cd => _cd.CreatedOn)
                  .Pipe(_q =>
                  {
                      var d = _q.Skip(pageSize * pageIndex).Take(pageSize).ToArray();
                      return new SequencePage<UserData>(d, includeCount ? _q.Count() : d.Length, pageSize, pageIndex);
                  });

        public bool UserExists(string userId)
        => _europa.Store<User>().Query.Any(_u => _u.EntityId == userId);
    }
}
