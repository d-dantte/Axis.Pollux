using Axis.Jupiter.Commands;
using Axis.Luna;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.Services.Queries;
using Axis.Pollux.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ValidatableExtensions;

namespace Axis.Pollux.Identity.Services
{
    public class UserManager: IUserManager
    {
        private IUserQuery _query = null;
        private IPersistenceCommands _pcommand = null;
        private IUserContext _userContext = null;

        public UserManager(IUserContext userContext, IUserQuery query, IPersistenceCommands pcommand)
        {
            ThrowNullArguments(() => query, 
                               () => pcommand,
                               () => userContext);

            _query = query;
            _pcommand = pcommand;
            _userContext = userContext;
        }

        #region Biodata
        public IOperation<BioData> UpdateBioData(BioData data)
        => ValidateModels(data).Then(() =>
        {
            var user = _userContext.User();
            var persisted = _query
                .GetBioData(user.UniqueId)
                .ThrowIf(_ => _.Owner.UniqueId != user.UniqueId, "Access Denied to data");

            if (persisted != null)
            {
                persisted.Dob = data.Dob;
                persisted.FirstName = data.FirstName;
                persisted.Gender = data.Gender;
                persisted.LastName = data.LastName;
                persisted.MiddleName = data.MiddleName;
                persisted.Nationality = data.Nationality;
                persisted.StateOfOrigin = data.StateOfOrigin;
                
                return _pcommand.Update(persisted);
            }
            else
            {
                data.Owner = user;
                return _pcommand.Add(data);
            }
        });

        public IOperation<BioData> GetBioData()
        => LazyOp.Try(() => _query.GetBioData(_userContext.User().UniqueId));
        #endregion

        #region Contact data
        public IOperation<ContactData> AddContactData(ContactChannel channel, string data)
        => LazyOp.Try(() =>
        {
            var contact = new ContactData
            {
                Value = data.ThrowIfNull("invalid data"),
                Channel = channel,
                Status = ContactStatus.Unverified,
                Owner = _userContext.User()
            };

            return _pcommand.Add(contact);
        });

        public IOperation DeleteContactData(long contactId)
        => LazyOp.Try(() =>
        {
            var data = _query
                .GetContactData(contactId)
                .ThrowIfNull("data not found")
                .ThrowIf(IsNotMyOwn, "Access Denied to data");

            _pcommand.Delete(data).Resolve();
        });

        public IOperation VerifyContactData(long contactId)
        => LazyOp.Try(() =>
        {
            var data = _query
                .GetContactData(contactId)
                .ThrowIfNull("data not found")
                .ThrowIf(IsNotMyOwn, "Access Denied to data")
                .ThrowIf(_c => _c.Status != ContactStatus.Unverified, "Invalid Contact status");

            data.Status = ContactStatus.Active;

            _pcommand.Update(data).Resolve();
        });

        public IOperation ArchiveContactData(long contactId)
        => LazyOp.Try(() =>
        {
            var data = _query
                .GetContactData(contactId)
                .ThrowIfNull("data not found")
                .ThrowIf(IsNotMyOwn, "Access Denied to data");

            data.Status = ContactStatus.Archived;

            _pcommand.Update(data).Resolve();
        });

        public IOperation<SequencePage<ContactData>> GetContactData(ContactStatus? status = null, PageParams pageParams = null)
        => LazyOp.Try(() =>
        {
            return _query.GetContactData(_userContext.User().UniqueId, status, pageParams);
        });

        public IOperation<ContactData> GetContactData(long id)
        => LazyOp.Try(() =>
        {
            return _query
                .GetContactData(id)
                .ThrowIf(IsNotMyOwn, "Access Denied to data");
        });

        public IOperation<SequencePage<ContactData>> GetContactDataOfType(ContactChannel channel, ContactStatus? status = null, PageParams pageParams = null)
        => LazyOp.Try(() =>
        {
            return _query.GetContactDataOfType(_userContext.User().UniqueId, channel, status, pageParams);
        });

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
                    if (_query.GetUserData(user.UniqueId, data.Name) != null) return null;

                    data.UniqueId = 0;
                    data.Owner = user;

                    return _pcommand.Add(data);
                })
                ?? LazyOp.Fail<UserData>(new NullReferenceException());
        });

        public IOperation<UserData> UpdateData(UserData data)
        => ValidateModels(data).Then(() =>
        {
            var user = _userContext.User();
            var persisted = _query.GetUserData(user.UniqueId, data.Name);
            if (persisted == null) return null;

            data.CopyTo(persisted,
                        nameof(UserData.UniqueId),
                        nameof(UserData.Owner),
                        nameof(UserData.CreatedOn),
                        nameof(UserData.ModifiedOn));

            return _pcommand.Update(persisted);
        });

        public IOperation<IEnumerable<UserData>> RemoveData(string[] names)
        => LazyOp.Try(() =>
        {
            var user = _userContext.User();
            return names
                .Select(_name => _query.GetUserData(user.UniqueId, _name))
                .Pipe(_data => _pcommand
                .DeleteBatch(_data)
                .Then(() => _data));
        });

        public IOperation<SequencePage<UserData>> GetUserData(PageParams pageParams)
        => LazyOp.Try(() => _query.GetUserData(_userContext.User().UniqueId, pageParams));

        public IOperation<UserData> GetUserData(string name)
        => LazyOp.Try(() => _query.GetUserData(_userContext.User().UniqueId, name));
        #endregion

        #region Address data
        public IOperation<AddressData> AddAddressData(AddressData address)
        => ValidateModels(address).Then(() =>
        {
            address.UniqueId = 0;
            address.Owner = _userContext.User();

            return _pcommand.Add(address);
        });

        public IOperation<AddressData> ModifyAddressData(AddressData address)
        => ValidateModels(address).Then(() =>
        {
            var persisted = _query
                .GetAddressById(address.UniqueId)
                .ThrowIfNull("invalid address")
                .ThrowIf(IsNotMyOwn, "Access Denied to data");

            address.CopyTo(persisted,
                           nameof(AddressData.UniqueId),
                           nameof(AddressData.Status),
                           nameof(AddressData.CreatedOn),
                           nameof(AddressData.Owner));

            return _pcommand.Update(persisted);
        });

        public IOperation<AddressData> ArchiveAddress(long id)
        => LazyOp.Try(() =>
        {
            var currentUser = _userContext.User();
            var persisted = _query
                .GetAddressById(id)
                .ThrowIfNull("invalid id")
                .ThrowIf(IsNotMyOwn, "Access Denied to data");

            persisted.Status = AddressStatus.Archived;
            return _pcommand.Update(persisted);
        });

        public IOperation<AddressData> ActivateAddress(long id)
        => LazyOp.Try(() =>
        {
            var currentUser = _userContext.User();
            var persisted = _query
                .GetAddressById(id)
                .ThrowIfNull("invalid id")
                .ThrowIf(IsNotMyOwn, "Access Denied to data");

            persisted.Status = AddressStatus.Active;
            return _pcommand.Update(persisted);
        });

        public IOperation<SequencePage<AddressData>> GetAddresses(AddressStatus? status, PageParams pageParams = null)
        => LazyOp.Try(() =>
        {
            return _query.GetAddresses(_userContext.User().UniqueId, status, pageParams);
        });

        public IOperation<AddressData> GetAddress(long id)
        => LazyOp.Try(() =>
        {
            return _query
                .GetAddressById(id)
                .ThrowIf(IsNotMyOwn, "Access Denied to data");
        });
        #endregion

        public IOperation<long> UserCount()
        => LazyOp.Try(() => _query.GetUserCount());

        public IOperation<User> CreateUser(string userName, int status)
        => LazyOp.Try(() =>
        {
            if (_query.UserExists(userName)) throw new Exception("the user name already exists in the system");

            else return _pcommand.Add(new User
            {
                UserName = userName,
                Status = status
            });            
        });

        private bool IsNotMyOwn(IUserOwned owned) => owned?.Owner?.UniqueId != _userContext.User().UniqueId;

        public IOperation<bool> UserIs(long userId, int status)
        => LazyOp.Try(() =>
        {
            return _query.UserIs(userId, status);
        });
    }
}
