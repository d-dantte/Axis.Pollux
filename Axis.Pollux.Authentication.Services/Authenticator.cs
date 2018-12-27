using System;
using System.Linq;
using Axis.Jupiter;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Contracts;
using Axis.Pollux.Authentication.Contracts.Params;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Authentication.Services.Queries;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Identity.Models;
using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Authentication.Services
{
    public class Authenticator: ICredentialManager
    {
        private readonly IAuthenticatorQueries _queries;
        private readonly StoreProvider _storeProvider;
        private readonly IDataAccessAuthorizer _authorizer;
        private readonly ICredentialHasher _hasher;

        public Authenticator(
            StoreProvider storeProvider, IDataAccessAuthorizer authorizer,
            IAuthenticatorQueries queries, ICredentialHasher hasher)
        {
            ThrowNullArguments(
                () => storeProvider,
                () => authorizer,
                () => queries,
                () => hasher);

            _queries = queries;
            _storeProvider = storeProvider;
            _authorizer = authorizer;
            _hasher = hasher;
        }


        public Operation ValidateNonUniqueCredential(NonUniqueCredentialValidationInfo info)
        => Operation.Try(async () =>
        {
            await info.Validate();

            //Ensure that the principal has access to this data
            await _authorizer.AuthorizeAccess(typeof(Credential).FullName, info.UserId);

            var request = ArrayPageRequest.CreateNormalizedRequest();
            var credentials = await _queries.GetActiveUserCredentials(info.UserId, info.Name, request); //<-- returns expired active credentials?
            while (credentials.Page.Length > 0)
            {
                if (credentials.Page.Any(credential => IsAuthenticated(credential, info.Data)))
                    return;

                else
                    credentials = await _queries.GetActiveUserCredentials(
                        info.UserId,
                        info.Name,
                        request = new ArrayPageRequest
                        {
                            PageIndex = request.PageIndex + 1,
                            PageSize = request.PageSize
                        });
            }

            throw new AuthenticationException(ErrorCodes.InvalidAuthenticationInfo);
        });

        public Operation<Guid> ValidateUniqueCredential(UniqueCredentialValidationInfo info)
        => Operation.Try(async () =>
        {
            await info.Validate();

            var request = ArrayPageRequest.CreateNormalizedRequest();
            var credentials = await _queries.GetActiveSystemUniqueCredentials(info.Name, request); //<-- returns expired active credentials?
            while (credentials.Page.Length > 0)
            {
                var credential = credentials.Page.FirstOrDefault(c => IsAuthenticated(c, info.Data));
                if (credential != null)
                {
                    //Ensure that the principal has access to this data. This privilege can (and should) be given to the Guest user
                    await _authorizer.AuthorizeAccess(typeof(Credential).FullName, credential.Owner.Id);

                    return credential.Owner.Id;
                }

                else
                    credentials = await _queries.GetActiveSystemUniqueCredentials(
                        info.Name,
                        request = new ArrayPageRequest
                        {
                            PageIndex = request.PageIndex + 1,
                            PageSize = request.PageSize
                        });
            }

            throw new AuthenticationException(ErrorCodes.InvalidAuthenticationInfo);
        });

        public Operation AddCredential(Guid userId, Credential credential)
        => Operation.Try(async () =>
        {
            await credential.Validate();

            if (userId == default(Guid))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidArgument);

            //data access validation
            await _authorizer.AuthorizeAccess(typeof(Credential).FullName, userId);

            credential.Id = Guid.NewGuid();
            credential.Owner = new User {Id = userId};

            //make sure uniqueness is not violated
            if (credential.Visibility == CredentialVisibility.Public)
            {
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
            await _authorizer.AuthorizeAccess(
                typeof(Credential).FullName,
                credential.Owner.Id,
                credential.Id.ToString());

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
            await _authorizer.AuthorizeAccess(
                typeof(Credential).FullName,
                credential.Owner.Id,
                credential.Id.ToString());

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
            await _authorizer.AuthorizeAccess(
                typeof(Credential).FullName,
                credential.Owner.Id,
                credential.Id.ToString());

            return credential.Status;
        });

        private bool IsAuthenticated(Credential credential, string inputData)
        {
            switch (credential.Visibility)
            {
                case CredentialVisibility.Public when string.Equals(credential.Data, inputData, StringComparison.InvariantCulture):
                    return true;

                case CredentialVisibility.Secret when IsValidHash(inputData, credential.Data):
                    return true;

                default:
                    return false;
            }
        }

        private bool IsValidHash(string plaintext, string b64Hash)
        => _hasher
            .ValidateHash(plaintext, b64Hash)
            .Then(() => true)
            .Catch(exception => false)
            .Resolve();
    }
}
