using System;
using Axis.Jupiter;
using Axis.Luna.Extensions;
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
    public class AddressDataManager: IAddressDataManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserQueries _userQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public AddressDataManager(
            IUserQueries userQueries, 
            IDataAccessAuthorizer dataAuthorizer, 
            StoreProvider storeProvider)
        {
            ThrowNullArguments(
                nameof(userQueries).ObjectPair(userQueries),
                nameof(dataAuthorizer).ObjectPair(dataAuthorizer),
                nameof(storeProvider).ObjectPair(storeProvider));

            _userQueries = userQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }

        #region AddressData

        public Operation<AddressData> AddAddressData(Guid userId, AddressData addressData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            await addressData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                 dataType: typeof(AddressData).FullName,
                 dataId: null,
                 intent: Authorization.Models.DataAccessIntent.Write));

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
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var addressData = (await _userQueries
                .GetAddressDataById(addressDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(AddressData).FullName,
                dataId: addressData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Delete));

            var storeCommand = _storeProvider.CommandFor(typeof(AddressData).FullName);

            return (await storeCommand
                .Delete(addressData))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<AddressData> UpdateAddressData(AddressData addressData)
        => Operation.Try(async () =>
        {
            await addressData
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            var persisted = (await _userQueries
                .GetAddressDataById(addressData.Id))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(AddressData).FullName,
                dataId: persisted.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

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
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var addressData = (await _userQueries
                .GetAddressDataById(addressDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType : typeof(AddressData).FullName,
                dataId : addressData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Write));

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
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var addressData = (await _userQueries
                .GetAddressDataById(addressDataId))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(
                dataType: typeof(AddressData).FullName,
                dataId: addressData.Id.ToString(),
                intent: Authorization.Models.DataAccessIntent.Read));

            return addressData;
        });

        public Operation<ArrayPage<AddressData>> GetUserAddresses(Guid userId, ArrayPageRequest request = null)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer
                .AuthorizeAccess(new Authorization.Models.DataAccessDescriptor(typeof(AddressData).FullName));

            return (await _userQueries
                .GetUserAddressData(userId, request))
                .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)); ;
        });

        #endregion
    }
}
