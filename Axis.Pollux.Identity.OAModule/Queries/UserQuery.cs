using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Services.Queries;
using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.UserCommon.Models;

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

        public AddressData GetAddressById(long id)
        => _europa
            .Query<AddressDataEntity>(_ad => _ad.Owner)
            .FirstOrDefault(_ad => _ad.UniqueId == id)
            .Transform<AddressDataEntity, AddressData>(_europa);

        public SequencePage<AddressData> GetAddresses(string userId, AddressStatus? status, PageParams pageParams = null)
        {
            var q = _europa
                .Query<AddressDataEntity>(_ad => _ad.Owner)
                .Where(_ad => _ad.OwnerId == userId);

            if (status.HasValue) q = q.Where(_ad => _ad.Status == status);

            q = q.OrderBy(_ad => _ad.CreatedOn);

            var d = q
                .Skip(pageParams.PageSize * pageParams.PageIndex)
                .Take(pageParams.PageSize)
                .Transform<AddressDataEntity, AddressData>(_europa)
                .ToArray();

            var count = pageParams.IncludeCount ? q.Count() : d.Length;
            return new SequencePage<AddressData>(d, count, pageParams.PageSize, pageParams.PageIndex);
        }

        public BioData GetBioData(string userId)
        => _europa.Query<BioDataEntity>(_bd => _bd.Owner)
                  .Where(_bd => _bd.OwnerId == userId)
                  .FirstOrDefault()
                  .Transform<BioDataEntity, BioData>(_europa);

        public UserData GetContactData(long id)
        => _europa.Query<UserDataEntity>(_cd => _cd.Owner)
                  .Where(_cd => _cd.UniqueId == id)
                  .FirstOrDefault()
                  .Transform<UserDataEntity, UserData>(_europa);

        public SequencePage<UserData> GetContactData(string userId, int? status, PageParams pageParams)
        {
            pageParams = pageParams ?? PageParams.EntireSequence();

            var q = _europa
                .Query<UserDataEntity>(_cd => _cd.Owner)
                .Where(_cd => _cd.OwnerId == userId);

            if (status.HasValue) q = q.Where(_cd => _cd.Status == status);

            q = q.OrderBy(_ad => _ad.CreatedOn);

            var d = q
                .Skip(pageParams.PageSize * pageParams.PageIndex)
                .Take(pageParams.PageSize)
                .Transform<UserDataEntity, UserData>(_europa)
                .ToArray();

            var count = pageParams.IncludeCount ? q.Count() : d.Length;
            return new SequencePage<UserData>(d, count, pageParams.PageSize, pageParams.PageIndex);
        }

        public long GetUserCount()
        => _europa.Query<UserEntity>().LongCount();

        public UserData GetUserData(string userId, string dataName)
        => _europa.Query<UserDataEntity>(_cd => _cd.Owner)
                  .Where(_cd => _cd.OwnerId == userId)
                  .Where(_cd => _cd.Name == dataName)
                  .FirstOrDefault()
                  .Pipe(new ModelConverter(_europa).ToModel<UserData>);

        public SequencePage<UserData> GetUserData(string userId, PageParams pageParams)
        => _europa
            .Query<UserDataEntity>(_cd => _cd.Owner)
            .Where(_cd => _cd.OwnerId == userId)
            .OrderBy(_cd => _cd.CreatedOn)
            .Pipe(_q =>
            {
                pageParams = pageParams ?? PageParams.EntireSequence();
                var converter = new ModelConverter(_europa);
                var d = _q
                  .Skip(pageParams.PageSize * pageParams.PageIndex)
                  .Take(pageParams.PageSize)
                  .AsEnumerable() //<-- pull from DB
                  .Select(converter.ToModel<UserData>)
                  .ToArray();

                var count = pageParams.IncludeCount ? _q.Count() : d.Length;
                return new SequencePage<UserData>(d, count, pageParams.PageSize, pageParams.PageIndex);
            });

        public bool UserExists(string userId)
        => _europa.Query<UserEntity>().Any(_u => _u.UniqueId == userId);
    }
}
