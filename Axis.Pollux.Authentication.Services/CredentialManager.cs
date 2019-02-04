using System;
using Axis.Jupiter;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Contracts;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Authentication.Services.AccessDescriptors;
using Axis.Pollux.Authentication.Services.Queries;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Identity.Models;

using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Authentication.Services
{
    public class CredentialManager : ICredentialManager
    {
        private readonly IAuthenticatorQueries _queries;
        private readonly StoreProvider _storeProvider;
        private readonly IDataAccessAuthorizer _authorizer;
        private readonly ICredentialHasher _hasher;

        public CredentialManager(
            StoreProvider storeProvider, 
            IDataAccessAuthorizer authorizer, 
            IAuthenticatorQueries queries,
            ICredentialHasher hasher)
        {
            ThrowNullArguments(
                () => storeProvider,
                () => authorizer,
                () => hasher,
                () => queries);

            _queries = queries;
            _storeProvider = storeProvider;
            _authorizer = authorizer;
            _hasher = hasher;
        }

        public Operation AddCredential(Guid userId, Credential credential)
        => Operation.Try(async () =>
        {
            await credential.Validate();

            if (userId == default(Guid))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //data access validation
            await _authorizer.AuthorizeAccess(new OwnedDataDescriptor
            {
                DataType = typeof(Credential).FullName,
                OwnerId = userId
            });

            credential.Id = Guid.NewGuid();
            credential.Owner = new User { Id = userId };

            switch (credential.Visibility)
            {
                //make sure uniqueness is not violated
                case CredentialVisibility.Public:
                    switch (credential.Uniqueness)
                    {
                        case Uniqueness.UserUnique when await _queries.ContainsPublicCredentials(userId, credential.Name, credential.Data):
                            throw new AuthenticationException(ErrorCodes.UniqueCredentialViolation);

                        case Uniqueness.SystemUnique when await _queries.ContainsPublicCredentials(credential.Name, credential.Data):
                            throw new AuthenticationException(ErrorCodes.UniqueCredentialViolation);

                        case Uniqueness.None:
                        default:
                            break;
                    }
                    break;

                //ensure data is hashed if necessary
                case CredentialVisibility.Secret:
                    credential.Data = await _hasher.CalculateHash(credential.Data);
                    break;

                default:
                    throw new AuthenticationException(Common.Exceptions.ErrorCodes.GeneralError);
            }

            (await _storeProvider
                .CommandFor(typeof(Credential).FullName)
                .ThrowIfNull(new AuthenticationException(ErrorCodes.UnmappedStoreCommandModelType))
                .Add(credential))
                .ThrowIfNull(new AuthenticationException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation UpdateCredentialStatus(Guid credentialId, int newStatus)
        => Operation.Try(async () =>
        {
            if (credentialId == default(Guid))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var credential = (await _queries
                .GetCredentialById(credentialId))
                .ThrowIfNull(new AuthenticationException(ErrorCodes.InvalidStoreQueryResult));

            //Ensure that the right principal has access to this data
            await _authorizer.AuthorizeAccess(new OwnedDataDescriptor
            {
                DataType = typeof(Credential).FullName,
                OwnerId = credential.Owner.Id,
                DataId = credential.Id.ToString()
            });

            credential.Status = newStatus;
            var storeCommand = _storeProvider.CommandFor(typeof(AddressData).FullName);

            (await storeCommand
                .Update(credential))
                .ThrowIfNull(new AuthenticationException(ErrorCodes.InvalidStoreCommandResult));
        });

        public Operation<bool> IsCredentialExpired(Guid credentialId)
        => Operation.Try(async () =>
        {
            if (credentialId == default(Guid))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var credential = await _queries.GetCredentialById(credentialId);

            //Ensure that the right principal has access to this data
            await _authorizer.AuthorizeAccess(new OwnedDataDescriptor
            {
                DataType = typeof(Credential).FullName,
                OwnerId = credential.Owner.Id,
                DataId = credential.Id.ToString()
            });

            return credential.ExpiresOn != null
                   && credential.ExpiresOn <= DateTimeOffset.Now;
        });

        public Operation<int> CredentialStatus(Guid credentialId)
        => Operation.Try(async () =>
        {
            if (credentialId == default(Guid))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidArgument);

            var credential = await _queries.GetCredentialById(credentialId);

            //Ensure that the right principal has access to this data
            await _authorizer.AuthorizeAccess(new OwnedDataDescriptor
            {
                DataType = typeof(Credential).FullName,
                OwnerId = credential.Owner.Id,
                DataId = credential.Id.ToString()
            });

            return credential.Status;
        });
    }
}
