﻿using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Services.Queries;
using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Common.Models;
using System;

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

            var oq = q.OrderBy(_ad => _ad.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(oq, _q => _q.Transform<AddressDataEntity, AddressData>(_europa));
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
                .Where(_cd => _cd.OwnerId == userId)
                .Where(_cd => _cd.Name.StartsWith("Contact: "));

            if (status.HasValue) q = q.Where(_cd => _cd.Status == status);

            var oq = q.OrderBy(_ad => _ad.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(oq, _q => _q.Transform<UserDataEntity, UserData>(_europa));
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

                return pageParams.Paginate(_q, __q => __q.Transform<UserDataEntity, UserData>(_europa));
            });

        public User GetUserById(string userId)
        => _europa
            .Query<UserEntity>()
            .FirstOrDefault(_u => _u.UniqueId == userId)
            .Transform<UserEntity, User>(_europa);

        public User GetUserUUID(Guid uuid)
        => _europa
            .Query<UserEntity>()
            .FirstOrDefault(_u => _u.GUId == uuid)
            .Transform<UserEntity, User>(_europa);

        public bool UserExists(string userId)
        => _europa.Query<UserEntity>().Any(_u => _u.UniqueId == userId);
    }
}
