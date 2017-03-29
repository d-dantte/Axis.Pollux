using Axis.Jupiter.Kore.Command;
using Axis.Luna;
using Axis.Luna.Extensions;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.Queries;
using Axis.Pollux.Owin.Common.Services;
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
        public Operation<BioData> UpdateBioData(BioData data)
        => Operation.Try(() =>
        {
            return data?
            .Validate()
            .Then(opr =>
            {
                var user = _userContext.CurrentUser();
                var persisted = _query.GetBioData(user.UserId);

                if (persisted != null)
                {
                    data.CopyTo(persisted,
                                nameof(BioData.EntityId),
                                nameof(BioData.OwnerId),
                                nameof(BioData.Owner),
                                nameof(BioData.CreatedOn),
                                nameof(BioData.ModifiedOn));

                    if (persisted.Dob <= DateTime.Parse("1753/1/1")) persisted.Dob = null;

                    return _pcommand.Update(persisted);
                }
                else return _pcommand.Add(data);
            })
            ?? Operation.Fail<BioData>(new NullReferenceException());            
        });

        public Operation<BioData> GetBioData()
        => Operation.Try(() => _query.GetBioData(_userContext.CurrentUser().UserId));
        #endregion

        #region Contact data
        public Operation<ContactData> AddContactData(ContactData data)
        => Operation.Try(() =>
        {
            return data?
            .Validate()
            .Then(opr =>
            {
                var user = _userContext.CurrentUser();
                data.OwnerId = user.UserId;
                return _pcommand.Add(data);
            })
            ?? Operation.Fail<ContactData>(new NullReferenceException());
        });
        public Operation<ContactData> UpdateContactData(ContactData data)
        => Operation.Try(() =>
        {
            return data?
            .Validate()
            .Then(opr =>
            {
                var user = _userContext.CurrentUser();
                var persisted = _query.GetContactData(data.EntityId);
                
                if (persisted == null) throw new Exception("invalid contact data");
                else if (persisted.OwnerId != user.UserId) throw new Exception("Access Denied");
                else
                {
                    data.CopyTo(persisted,
                                nameof(ContactData.OwnerId),
                                nameof(ContactData.Owner),
                                nameof(ContactData.CreatedOn),
                                nameof(ContactData.ModifiedOn));

                    return _pcommand.Update(persisted);
                }
            })
            ?? Operation.Fail<ContactData>(new NullReferenceException());
        });

        public Operation<ContactData> ArchiveContactData(long id)
        => Operation.Try(() =>
        {
            var user = _userContext.CurrentUser();
            var persisted = _query.GetContactData(id);

            if (persisted == null) throw new Exception("invalid contact data");
            else if (persisted.OwnerId != user.UserId) throw new Exception("Access Denied");
            else
            {
                persisted.Status = ContactStatus.Archived;
                return _pcommand.Update(persisted);
            }
        });

        public Operation<SequencePage<ContactData>> GetAllContactData(int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => Operation.Try(() => _query.GetContactData(_userContext.CurrentUser().UserId, pageSize, pageIndex, includeCount));
        #endregion

        #region User data
        public Operation<UserData> AddData(UserData data)
        => Operation.Try(() =>
        {
            return data?
            .Validate()
            .Then(opr =>
            {
                var user = _userContext.CurrentUser();
                if (_query.GetUserData(user.UserId, data.Name) != null) return null;

                data.EntityId = 0;
                data.Owner = user;
                data.OwnerId = user.UserId;

                return _pcommand.Add(data);
            })
            ?? Operation.Fail<UserData>(new NullReferenceException());
        });

        public Operation<UserData> UpdateData(UserData data)
        => Operation.Try(() =>
        {
            return data?
            .Validate()
            .Then(opr =>
            {
                var user = _userContext.CurrentUser();
                var persisted = _query.GetUserData(user.UserId, data.Name);
                if (persisted == null) return null;

                data.CopyTo(persisted,
                            nameof(UserData.Owner),
                            nameof(UserData.OwnerId),
                            nameof(UserData.CreatedOn),
                            nameof(UserData.ModifiedOn));

                return _pcommand.Update(data).Resolve();
            })
            ?? Operation.Fail<UserData>(new NullReferenceException());
        });

        public Operation<IEnumerable<UserData>> RemoveData(string[] names)
        => Operation.Try(() =>
        {
            var user = _userContext.CurrentUser();
            return names.Select(_name =>
            {
                var data = _query.GetUserData(user.UserId, _name);
                if (data == null) return null;
                else return _pcommand.Delete(data).Resolve();
            })
            .Where(_data => _data != null)
            .ToArray()
            .AsEnumerable();
        });

        public Operation<SequencePage<UserData>> GetUserData(int pageSize = 500, int pageIndex = 0, bool includeCount = true)
        => Operation.Try(() => _query.GetUserData(_userContext.CurrentUser().UserId, pageSize, pageIndex, includeCount));

        public Operation<UserData> GetUserData(string name)
        => Operation.Try(() => _query.GetUserData(_userContext.CurrentUser().UserId, name));
        #endregion

        public Operation<long> UserCount()
        => Operation.Try(() => _query.GetUserCount());

        public Operation<User> CreateUser(string userId, int status)
        => Operation.Try(() =>
        {
            if (_query.UserExists(userId)) throw new Exception("the user id already exists in the system");

            else return _pcommand.Add(new User
            {
                EntityId = userId,
                Status = status
            });            
        });
    }
}
