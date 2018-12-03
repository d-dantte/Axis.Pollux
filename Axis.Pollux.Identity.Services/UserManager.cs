using System;
using System.Collections.Generic;
using System.Text;
using Axis.Jupiter;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Exceptions;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.Queries;

using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Identity.Services
{
    public class ProfileManager: IProfileManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserQueries _userQueries;
        private readonly IUserContext _userContext;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public ProfileManager(IUserContext userContext, IUserQueries userQueries, 
                              IDataAccessAuthorizer dataAuthorizer, StoreProvider storeProvider)
        {
            ThrowNullArguments(() => userContext,
                               () => userQueries,
                               () => dataAuthorizer,
                               () => storeProvider);

            _userContext = userContext;
            _userQueries = userQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }


        #region User

        public Operation<User> CreateUser(int? status = null)
        => Operation.Try(async () =>
        {
            var user = new User { Status =  status ?? 0 };
            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);
            return await storeCommand.Add(user);
        });

        public Operation<User> DeleteUser(Guid userId)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new ArgumentException("Invalid id specified");

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(User).FullName, userId.ToString(), userId);

            var user = await _userQueries.GetUserById(userId);
            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);
            user = await storeCommand.Delete(user);

            return user ?? throw new IdentityException(ErrorCodes.InvalidStoreCommandResult);
        });
        
        public Operation UpdateUserStatus(Guid userId, int status)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new ArgumentException("Invalid id specified");

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(User).FullName, userId.ToString(), userId);

            var user = await _userQueries.GetUserById(userId);
            user.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);
            user = await storeCommand.Update(user);

            if(user == null)
                throw new IdentityException(ErrorCodes.InvalidStoreCommandResult);
        });

        public Operation<User> GetUser(Guid userId)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new ArgumentException("Invalid id specified");

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(User).FullName, userId.ToString(), userId);

            var user = await _userQueries.GetUserById(userId);

            return user ?? throw new IdentityException(ErrorCodes.InvalidStoreCommandResult);
        });


        public Operation<UserProfile> GetUserProfile(Guid userId)
        => Operation.Try(async () =>
        {
            //retrieve the object, then one by one, retrieve it's constituents
        });

        public Operation<long> UserCount()
        => Operation.Try(async () =>
        {
            var count = await _userQueries.UserCount();
            return count.ThrowIf(c => c < 0, new IdentityException(ErrorCodes.InvalidStoreQueryResult));
        });

        #endregion

        #region BioData

        Operation<BioData> CreateBioData(Guid userId, BioData bioData);
        Operation<BioData> DeleteBioData(Guid bioDataId);
        Operation<BioData> UpdateBioData(BioData bioData);
        Operation UpdateBioDataStatus(Guid bioDataId, int status);

        Operation<BioData> GetUserBioData(Guid bioDataId);

        #endregion

        #region AddressData

        Operation<AddressData> AddAddressData(Guid userId, AddressData addressData);
        Operation<AddressData> DeleteAddressData(Guid addressDataId);
        Operation<AddressData> UpdateAddressData(AddressData addressData);
        Operation UpdateAddressDataStatus(Guid addressDataId, int status);

        Operation<AddressData> GetAddressData(Guid addressDataId);
        Operation<ArrayPage<AddressData>> GetUserAddresses(Guid userId, ArrayPageRequest request = null);

        #endregion

        #region ContactData

        Operation<ContactData> AddContactData(Guid userId, ContactData contactData);
        Operation<ContactData> DeleteContactData(Guid contactDataId);
        Operation<ContactData> UpdateContactData(ContactData contactData);
        Operation UpdateContactDataStatus(Guid contactDataId, int status);

        Operation<ContactData> GetContactData(Guid contactDataId);
        Operation<ArrayPage<ContactData>> GetUserContact(Guid userId, ArrayPageRequest request = null);

        #endregion

        #region NameData

        Operation<NameData> AddNameData(Guid userId, NameData nameData);
        Operation<NameData> DeleteNameData(Guid nameDataId);
        Operation<NameData> UpdateNameData(NameData nameData);
        Operation UpdateNameDataStatus(Guid nameDataId, int status);

        Operation<NameData> GetNameData(Guid nameDataId);
        Operation<ArrayPage<NameData>> GetUserName(Guid userId, ArrayPageRequest request = null);

        #endregion

        #region UserData

        Operation<UserData> AddUserData(Guid userId, UserData userData);
        Operation<UserData> DeleteUserData(Guid userDataId);
        Operation<UserData> UpdateUserData(UserData userData);
        Operation UpdateUserDataStatus(Guid userDataId, int status);

        Operation<UserData> GetUserData(Guid userDataId);
        Operation<ArrayPage<UserData>> GetUserUser(Guid userId, ArrayPageRequest request = null);

        #endregion
    }
}
