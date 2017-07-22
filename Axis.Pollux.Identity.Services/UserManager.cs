using Axis.Jupiter.Commands;
using Axis.Luna;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.Services.Queries;
using Axis.Pollux.UserCommon.Models;
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
                .GetBioData(user.UserId)
                .ThrowIf(_ => _.Owner.UserId != user.UserId, "Access Denied to data");

            if (persisted != null)
            {
                data.CopyTo(persisted,
                            nameof(BioData.UniqueId),
                            nameof(BioData.Owner),
                            nameof(BioData.CreatedOn));

                if (persisted.Dob <= DateTime.Parse("1753/1/1")) persisted.Dob = null;

                return _pcommand.Update(persisted);
            }
            else
            {
                data.Owner = user;
                return _pcommand.Add(data);
            }
        });

        public IOperation<BioData> GetBioData()
        => LazyOp.Try(() => _query.GetBioData(_userContext.User().UserId));
        #endregion

        #region Contact data
        public IOperation<UserData> AddContactData(string contactType, string data, string label = null)
        => LazyOp.Try(() =>
        {
            var userData = new UserData
            {
                Data = data.ThrowIfNull("invalid data"),
                Label = label,
                Name = contactType,
                Type = contactType == Constants.ContactType_Email? Luna.Utils.CommonDataType.Email:
                       contactType == Constants.ContactType_Phone? Luna.Utils.CommonDataType.Phone:
                       Luna.Utils.CommonDataType.String,
                Status = Constants.ContactStatus_PendingValidation,
                Owner = _userContext.User()
            };

            return _pcommand.Add(userData);
        });

        public IOperation<UserData> DeleteContactData(long id)
        => LazyOp.Try(() =>
        {
            var data = _query
                .GetContactData(id)
                .ThrowIfNull("data not found")
                .ThrowIf(IsNotMyOwn, "Access Denied to data");

            return _pcommand.Delete(data);
        });

        public IOperation<UserData> UpdateContactStatus(long id, int status)
        => LazyOp.Try(() =>
        {
            var data = _query
                .GetContactData(id)
                .ThrowIfNull("data not found")
                .ThrowIf(IsNotMyOwn, "Access Denied to data");

            data.Status = status;

            return _pcommand.Update(data);
        });

        public IOperation<SequencePage<UserData>> GetContactData(int? status = Constants.ContactStatus_Validated, PageParams pageParams = null)
        => LazyOp.Try(() =>
        {
            return _query.GetContactData(_userContext.User().UserId, status, pageParams);
        });

        public IOperation<UserData> GetContactData(long id)
        => LazyOp.Try(() =>
        {
            return _query
                .GetContactData(id)
                .ThrowIf(IsNotMyOwn, "Access Denied to data");
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
                    if (_query.GetUserData(user.UserId, data.Name) != null) return null;

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
            var persisted = _query.GetUserData(user.UserId, data.Name);
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
                .Select(_name => _query.GetUserData(user.UserId, _name))
                .Pipe(_data => _pcommand
                .DeleteBatch(_data)
                .Then(() => _data));
        });

        public IOperation<SequencePage<UserData>> GetUserData(PageParams pageParams)
        => LazyOp.Try(() => _query.GetUserData(_userContext.User().UserId, pageParams));

        public IOperation<UserData> GetUserData(string name)
        => LazyOp.Try(() => _query.GetUserData(_userContext.User().UserId, name));
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
            return _query.GetAddresses(_userContext.User().UserId, status, pageParams);
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

        private bool IsNotMyOwn(IUserOwned owned) => owned?.Owner?.UserId != _userContext.User().UserId;
    }
}
