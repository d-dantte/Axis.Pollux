using System;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Contracts;
using Axis.Pollux.Authentication.Contracts.Params;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Authentication.Services.AccessDescriptors;
using Axis.Pollux.Authentication.Services.Queries;
using Axis.Pollux.Authorization.Contracts;
using Axis.Pollux.Common.Utils;
using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Authentication.Services
{
    public class Authenticator: ICredentialValidator
    {
        private readonly IAuthenticatorQueries _queries;
        private readonly IDataAccessAuthorizer _authorizer;
        private readonly ICredentialHasher _hasher;

        public Authenticator(IDataAccessAuthorizer authorizer, IAuthenticatorQueries queries, ICredentialHasher hasher)
        {
            ThrowNullArguments(
                () => authorizer,
                () => queries,
                () => hasher);

            _queries = queries;
            _authorizer = authorizer;
            _hasher = hasher;
        }


        public Operation ValidateNonUniqueCredential(NonUniqueCredentialValidationInfo info)
        => Operation.Try(async () =>
        {
            await info.Validate();

            //Ensure that the principal has access to this data
            await _authorizer.AuthorizeAccess(new OwnedDataDescriptor
            {
                DataType = typeof(Credential).FullName,
                OwnerId = info.UserId
            });

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
                    await _authorizer.AuthorizeAccess(new OwnedDataDescriptor
                    {
                        DataType = typeof(Credential).FullName,
                        OwnerId = credential.Owner.Id
                    });

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


        private bool IsAuthenticated(Credential credential, string inputData)
        {
            switch (credential.Visibility)
            {
                case CredentialVisibility.Public 
                when string.Equals(credential.Data, inputData, StringComparison.InvariantCulture):
                    return true;

                case CredentialVisibility.Secret 
                when IsValidHash(inputData, credential.Data):
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
