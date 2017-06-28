using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Services.Queries;
using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;

namespace Axis.Pollux.Identity.OAModule.Queries
{
    public class UserQuery: IUserQuery
    {
        private DataStore _europa = null;

        public UserQuery(DataStore context)
        {
            ThrowNullArguments(() => context);

            _europa = context;
        }

        public BioData GetBioData(string userId)
        => _europa.Query<BioDataEntity>(_bd => _bd.Owner)
                  .Where(_bd => _bd.OwnerId == userId)
                  .FirstOrDefault()
                  .Pipe(new ModelConverter(_europa).ToModel<BioData>);

        public ContactData GetContactData(long id)
        => _europa.Query<ContactDataEntity>(_cd => _cd.Owner)
                  .Where(_cd => _cd.UniqueId == id)
                  .FirstOrDefault()
                  .Pipe(new ModelConverter(_europa).ToModel<ContactData>);

        public SequencePage<ContactData> GetContactData(string userId, int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => _europa.Query<ContactDataEntity>(_cd => _cd.Owner)
                  .Where(_cd => _cd.OwnerId == userId)
                  .OrderBy(_cd => _cd.CreatedOn)
                  .Pipe(_q =>
                  {
                      var converter = new ModelConverter(_europa);
                      var d = _q
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .AsEnumerable() //<-- pull from DB
                        .Select(converter.ToModel<ContactData>)
                        .ToArray();

                      var count = includeCount ? _q.Count() : d.Length;
                      return new SequencePage<ContactData>(d, count, pageSize, pageIndex);
                  });

        public long GetUserCount()
        => _europa.Query<UserEntity>().LongCount();

        public UserData GetUserData(string userId, string dataName)
        => _europa.Query<UserDataEntity>(_cd => _cd.Owner)
                  .Where(_cd => _cd.OwnerId == userId)
                  .Where(_cd => _cd.Name == dataName)
                  .FirstOrDefault()
                  .Pipe(new ModelConverter(_europa).ToModel<UserData>);

        public SequencePage<UserData> GetUserData(string userId, int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => _europa.Query<UserDataEntity>(_cd => _cd.Owner)
                  .Where(_cd => _cd.OwnerId == userId)
                  .OrderBy(_cd => _cd.CreatedOn)
                  .Pipe(_q =>
                  {
                      var converter = new ModelConverter(_europa);
                      var d = _q
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .AsEnumerable() //<-- pull from DB
                        .Select(converter.ToModel<UserData>)
                        .ToArray();

                      var count = includeCount ? _q.Count() : d.Length;
                      return new SequencePage<UserData>(d, count, pageSize, pageIndex);
                  });

        public bool UserExists(string userId)
        => _europa.Query<UserEntity>().Any(_u => _u.UniqueId == userId);
    }
}
