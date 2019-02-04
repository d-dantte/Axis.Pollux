using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Identity.Contracts;
using Axis.Pollux.Identity.Exceptions;
using Axis.Pollux.Identity.Models;
using Axis.Pollux.Identity.Services.AccessDescriptors;
using Axis.Pollux.Identity.Services.Queries;

using static Axis.Luna.Extensions.ExceptionExtension;


namespace Axis.Pollux.Identity.Services
{
    public class ProfileManager: IProfileManager
    {
        private readonly IUserQueries _userQueries;
        private readonly IDataAccessAuthorizer _dataAccessAuthorizer;


        public ProfileManager(IUserQueries userQueries, IDataAccessAuthorizer dataAuthorizer)
        {
            ThrowNullArguments(() => userQueries,
                () => dataAuthorizer);

            _userQueries = userQueries;
            _dataAccessAuthorizer = dataAuthorizer;
        }


        public Operation<UserProfile> GetUserProfile(Contracts.Params.UserProfileRequestInfo param)
        => Operation.Try(async () =>
        {
            await param
                .ThrowIfNull(new IdentityException(Common.Exceptions.ErrorCodes.InvalidArgument))
                .Validate();

            //Ensure that the right principal has access to this data
            await _dataAccessAuthorizer.AuthorizeAccess(new OwnedDataDescriptor
            {
                DataType = typeof(UserProfile).FullName,
                OwnerId = param.UserId
            });

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
                    .GetUserData(param.UserId, param.UserDataRequest))
                    .ThrowIfNull(new IdentityException(ErrorCodes.InvalidStoreQueryResult)),

                //a null bio is valid
                Bio = await _userQueries.GetUserBioData(param.UserId)
            };
        });
    }
}
