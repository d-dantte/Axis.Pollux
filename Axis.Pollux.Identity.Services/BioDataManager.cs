using System;
using Axis.Jupiter;
using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Exceptions;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.Queries;

using static Axis.Luna.Extensions.ExceptionExtension;


namespace Axis.Pollux.Identity.Services
{
    public class BioDataManager: IBioDataManager
    {
        private readonly StoreProvider _storeProvider;
        private readonly IUserQueries _userQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public BioDataManager(IUserQueries userQueries, IDataAccessAuthorizer dataAuthorizer, StoreProvider storeProvider)
        {
            ThrowNullArguments(() => userQueries,
                () => dataAuthorizer,
                () => storeProvider);

            _userQueries = userQueries;
            _storeProvider = storeProvider;
            _dataAccessAuthorizer = dataAuthorizer;
        }

        #region BioData

        public Operation<BioData> CreateBioData(Guid userId, BioData bioData)
        => Operation.Try(async () =>
        {
            if (userId == default(Guid))
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

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
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

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
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
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
                throw new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument);

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
    }
}
