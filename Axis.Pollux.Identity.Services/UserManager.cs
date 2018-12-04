using System;
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
            var user = new User
            {
                Id = Guid.NewGuid(),
                Status =  status ?? 0
            };
            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);

            return (await storeCommand
                .Add(user))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<User> DeleteUser(Guid userId)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(User).FullName, userId, userId.ToString());

            var user = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);

            return (await storeCommand
                .Delete(user))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });
        
        public Operation UpdateUserStatus(Guid userId, int status)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(User).FullName, userId, userId.ToString());

            var user = (await _userQueries
                    .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            user.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);

            (await storeCommand
                .Update(user))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<User> GetUser(Guid userId)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(User).FullName, 
                userId, 
                userId.ToString());

            return (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));
        });

        public Operation<UserProfile> GetUserProfile(Contracts.Params.UserProfileRequest param)
        => Operation.Try(async () =>
        {
            await param
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(UserProfile).FullName, param.UserId);

            //retrieve the object, then one by one, retrieve it's constituents
            return new UserProfile
            {
                User = (await _userQueries
                    .GetUserById(param.UserId))
                    .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)),

                Addresses = (await _userQueries
                    .GetUserAddressData(param.UserId, param.AddressDataRequest))
                    .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)),

                ContactInfo = (await _userQueries
                    .GetUserContactData(param.UserId, param.ContactDataRequest))
                    .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)),

                Names = (await _userQueries
                    .GetUserNameData(param.UserId, param.NameDataRequest))
                    .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)),

                Data = (await _userQueries
                    .GetUserUserData(param.UserId, param.UserDataRequest))
                    .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)),

                //a null bio is valid
                Bio = await _userQueries.GetUserBioData(param.UserId)
            };
        });

        public Operation<long> UserCount()
        => Operation.Try(async () =>
        {
            var count = await _userQueries.UserCount();
            return count.ThrowIf(c => c < 0, new IdentityException(ErrorCodes.InvalidStoreQueryResult));
        });

        #endregion

        #region BioData

        public Operation<BioData> CreateBioData(Guid userId, BioData bioData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(BioData).FullName, userId);

            bioData.Id = Guid.NewGuid();
            bioData.Owner = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(User).FullName);

            return (await storeCommand
                .Add(bioData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<BioData> DeleteBioData(Guid bioDataId)
        => Operation.Try(async () =>
        {
            if (bioDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var bioData = (await _userQueries
                .GetBioDataById(bioDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(BioData).FullName,
                bioData.Owner.Id,
                bioData.Id.ToString());

            var storeCommand = _storeProvider.CommandFor(typeof(BioData).FullName);

            return (await storeCommand
                .Delete(bioData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<BioData> UpdateBioData(BioData bioData)
        => Operation.Try(async () =>
        {
            await bioData
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidArgument))
                .Validate();

            var persisted = (await _userQueries
                .GetUserBioData(bioData.Id))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(BioData).FullName,
                persisted.Owner.Id,
                persisted.Id.ToString());

            //copy values
            persisted.CountryOfBirth = bioData.CountryOfBirth;
            persisted.DateOfBirth = bioData.DateOfBirth;
            persisted.Gender = bioData.Gender;

            var storeCommand = _storeProvider.CommandFor(typeof(BioData).FullName);
            return (await storeCommand
                .Update(persisted))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<BioData> GetUserBioData(Guid userId)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var bioData = (await _userQueries
                .GetUserBioData(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(BioData).FullName,
                userId,
                bioData.Id.ToString());

            return bioData;
        });

        #endregion

        #region AddressData

        public Operation<AddressData> AddAddressData(Guid userId, AddressData addressData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            await addressData
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(AddressData).FullName, userId);

            addressData.Id = Guid.NewGuid();
            addressData.Owner = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(AddressData).FullName);

            return (await storeCommand
                .Add(addressData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<AddressData> DeleteAddressData(Guid addressDataId)
        => Operation.Try(async () =>
        {
            if (addressDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var addressData = (await _userQueries
                .GetAddressDataById(addressDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(AddressData).FullName,
                addressData.Owner.Id,
                addressData.Id.ToString());

            var storeCommand = _storeProvider.CommandFor(typeof(AddressData).FullName);

            return (await storeCommand
                .Delete(addressData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<AddressData> UpdateAddressData(AddressData addressData)
        => Operation.Try(async () =>
        {
            await addressData
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidArgument))
                .Validate();

            var persisted = (await _userQueries
                .GetAddressDataById(addressData.Id))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(AddressData).FullName,
                persisted.Owner.Id,
                persisted.Id.ToString());

            //copy values
            persisted.City = addressData.City;
            persisted.Country = addressData.Country;
            persisted.Flat = addressData.Flat;
            persisted.PostCode = addressData.PostCode;
            persisted.StateProvince = addressData.StateProvince;
            persisted.Street = addressData.Street;

            var storeCommand = _storeProvider.CommandFor(typeof(AddressData).FullName);
            return (await storeCommand
                .Update(persisted))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation UpdateAddressDataStatus(Guid addressDataId, int status)
        => Operation.Try(async () =>
        {
            if (addressDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var addressData = (await _userQueries
                .GetAddressDataById(addressDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(AddressData).FullName, 
                addressData.Owner.Id,
                addressData.Id.ToString());

            addressData.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(AddressData).FullName);

            (await storeCommand
                .Update(addressData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<AddressData> GetAddressData(Guid addressDataId)
        => Operation.Try(async () =>
        {
            if (addressDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var addressData = (await _userQueries
                .GetAddressDataById(addressDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(AddressData).FullName,
                addressData.Owner.Id,
                addressData.Id.ToString());

            return addressData;
        });

        public Operation<ArrayPage<AddressData>> GetUserAddresses(Guid userId, ArrayPageRequest request = null)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(AddressData).FullName,
                userId);

            return (await _userQueries
                .GetUserAddressData(userId, request))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        #endregion

        #region ContactData

        public Operation<ContactData> AddContactData(Guid userId, ContactData contactData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            await contactData
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(typeof(ContactData).FullName, userId);

            contactData.Id = Guid.NewGuid();
            contactData.Owner = (await _userQueries
                .GetUserById(userId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            return (await storeCommand
                .Add(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> DeleteContactData(Guid contactDataId)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(AddressData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            return (await storeCommand
                .Delete(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> UpdateContactData(ContactData contactData)
        => Operation.Try(async () =>
        {
            await contactData
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidArgument))
                .Validate();

            var persisted = (await _userQueries
                .GetContactDataById(contactData.Id))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                persisted.Owner.Id,
                persisted.Id.ToString());

            //copy values
            persisted.Channel = contactData.Channel;
            persisted.Data = contactData.Data;
            persisted.IsPrimary = contactData.IsPrimary;

            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);
            return (await storeCommand
                .Update(persisted))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation UpdateContactDataStatus(Guid contactDataId, int status)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                    .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

            contactData.Status = status;
            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation VerifyContactData(Guid contactDataId)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                    .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

            contactData.IsVerified = true;
            var storeCommand = _storeProvider.CommandFor(typeof(ContactData).FullName);

            (await storeCommand
                .Update(contactData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<ContactData> GetContactData(Guid contactDataId)
        => Operation.Try(async () =>
        {
            if (contactDataId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            var contactData = (await _userQueries
                .GetContactDataById(contactDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                contactData.Owner.Id,
                contactData.Id.ToString());

            return contactData;
        });

        public Operation<ArrayPage<ContactData>> GetUserContact(Guid userId, ArrayPageRequest request = null)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(
                typeof(ContactData).FullName,
                userId);

            return (await _userQueries
                .GetUserContactData(userId, request))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        #endregion

        #region NameData

        public Operation<NameData> AddNameData(Guid userId, NameData nameData);
        public Operation<NameData> DeleteNameData(Guid nameDataId);
        public Operation<NameData> UpdateNameData(NameData nameData);
        public Operation UpdateNameDataStatus(Guid nameDataId, int status);

        public Operation<NameData> GetNameData(Guid nameDataId);
        public Operation<ArrayPage<NameData>> GetUserName(Guid userId, ArrayPageRequest request = null);

        #endregion

        #region UserData

        public Operation<UserData> AddUserData(Guid userId, UserData userData);
        public Operation<UserData> DeleteUserData(Guid userDataId);
        public Operation<UserData> UpdateUserData(UserData userData);
        public Operation UpdateUserDataStatus(Guid userDataId, int status);

        public Operation<UserData> GetUserData(Guid userDataId);
        public Operation<ArrayPage<UserData>> GetUserUser(Guid userId, ArrayPageRequest request = null);

        #endregion


        #region Contract params
        #endregion
    }
}
