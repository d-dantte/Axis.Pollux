using Axis.Jupiter.Commands;
using Axis.Luna;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Services;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.Services.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.Identity.Services
{
    public class UserManager: IUserManager
    {
        private IUserQuery _query = null;
        private IPersistenceCommands _pcommand = null;
        private IUserContext _userContext = null;

        public UserManager(IUserQuery query, IPersistenceCommands pcommand)
        {
            ThrowNullArguments(() => query, () => pcommand);

            _query = query;
            _pcommand = pcommand;
        }

        #region Biodata
        public IOperation<BioData> UpdateBioData(BioData data)
        => LazyOp.Try(() =>
        {
            return data?
                .Validate()
                .Then(() =>
                {
                    var user = _userContext.User();
                    var persisted = _query.GetBioData(user.UserId);

                    if (persisted != null)
                    {
                        data.CopyTo(persisted,
                                    nameof(ContactData.Owner),
                                    nameof(ContactData.CreatedOn),
                                    nameof(ContactData.ModifiedOn));

                        if (persisted.Dob <= DateTime.Parse("1753/1/1")) persisted.Dob = null;

                        return _pcommand.Update(persisted);
                    }
                    else
                    {
                        data.Owner = user;
                        return _pcommand.Add(data);
                    }
                })
                ?? LazyOp.Fail<BioData>(new NullReferenceException());            
        });

        public IOperation<BioData> GetBioData()
        => LazyOp.Try(() => _query.GetBioData(_userContext.User().UserId));
        #endregion

        #region Contact data
        public IOperation<ContactData> AddContactData(ContactData data)
        => LazyOp.Try(() =>
        {
            return data?
                .Validate()
                .Then(() =>
                {
                    var user = _userContext.User();
                    data.Owner = user;
                    return _pcommand.Add(data);
                })
                ?? LazyOp.Fail<ContactData>(new NullReferenceException());
        });

        public IOperation<ContactData> UpdateContactData(ContactData data)
        => LazyOp.Try(() =>
        {
            return data?
                .Validate()
                .Then(() =>
                {
                    var user = _userContext.User();
                    var persisted = _query.GetContactData(data.UniqueId);
                    
                    if (persisted == null) throw new Exception("invalid contact data");
                    else if (persisted.Owner.UserId != user.UserId) throw new Exception("Access Denied");
                    else
                    {
                        data.CopyTo(persisted,
                                    nameof(ContactData.Owner),
                                    nameof(ContactData.CreatedOn),
                                    nameof(ContactData.ModifiedOn));

                        return _pcommand.Update(persisted);
                    }
                })
                ?? LazyOp.Fail<ContactData>(new NullReferenceException());
        });

        public IOperation<ContactData> ArchiveContactData(long id)
        => LazyOp.Try(() =>
        {
            var user = _userContext.User();
            var persisted = _query.GetContactData(id);

            if (persisted == null) throw new Exception("invalid contact data");
            else if (persisted.Owner.UserId != user.UserId) throw new Exception("Access Denied");
            else
            {
                persisted.Status = ContactStatus.Archived;
                return _pcommand.Update(persisted);
            }
        });

        public IOperation<SequencePage<ContactData>> GetAllContactData(int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => LazyOp.Try(() => _query.GetContactData(_userContext.User().UserId, pageSize, pageIndex, includeCount));
        #endregion

        #region User data
        public IOperation<UserData> AddData(UserData data)
        => LazyOp.Try(() =>
        {
            return data?
                .Validate()
                .Then(() =>
                {
                    var user = _userContext.User();
                    if (_query.GetUserData(user.UserId, data.Name) != null) return null;

                    data.UniqueId = 0;
                    data.Owner = user;

                    return _pcommand.Add(data);
                })
                ?? LazyOp.Fail<UserData>(new NullReferenceException());
        });

        public IOperation<UserData> UpdateData(UserData data)
        => LazyOp.Try(() =>
        {
            return data?
                .Validate()
                .Then(() =>
                {
                    var user = _userContext.User();
                    var persisted = _query.GetUserData(user.UserId, data.Name);
                    if (persisted == null) return null;

                    data.CopyTo(persisted,
                                nameof(UserData.Owner),
                                nameof(UserData.CreatedOn),
                                nameof(UserData.ModifiedOn));

                    return _pcommand.Update(persisted);
                })
                ?? LazyOp.Fail<UserData>(new NullReferenceException());
        });

        public IOperation<IEnumerable<UserData>> RemoveData(string[] names)
        => LazyOp.Try(() =>
        {
            var user = _userContext.User();
            return names
                .Select(_name => _query.GetUserData(user.UserId, _name))
                .Pipe(_data => _pcommand
                .DeleteBatch(_data)
                .Then(() => _data));
        });

        public IOperation<SequencePage<UserData>> GetUserData(int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => LazyOp.Try(() => _query.GetUserData(_userContext.User().UserId, pageSize, pageIndex, includeCount));

        public IOperation<UserData> GetUserData(string name)
        => LazyOp.Try(() => _query.GetUserData(_userContext.User().UserId, name));
        #endregion

        public IOperation<long> UserCount()
        => LazyOp.Try(() => _query.GetUserCount());

        public IOperation<User> CreateUser(string userId, int status)
        => LazyOp.Try(() =>
        {
            if (_query.UserExists(userId)) throw new Exception("the user id already exists in the system");

            else return _pcommand.Add(new User
            {
                UniqueId = userId,
                Status = status
            });            
        });
    }
}
