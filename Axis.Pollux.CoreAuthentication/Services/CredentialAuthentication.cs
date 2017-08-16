using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.OperationExtensions;

using System;
using System.Linq;
using Axis.Pollux.Authentication;
using Axis.Pollux.Authentication.Services;
using Axis.Pollux.CoreAuthentication.Queries;
using Axis.Luna.Operation;
using Axis.Luna.Extensions;
using Axis.Jupiter.Commands;
using Axis.Pollux.Authentication.Models;

namespace Axis.Pollux.CoreAuthentication.Services
{
    public class CredentialAuthentication: ICredentialAuthority
    {
        private IAuthenticationQuery _query;
        private IPersistenceCommands _pcommand;

        public ICredentialHasher CredentialHasher { get; private set; }
        
        public CredentialAuthentication(IAuthenticationQuery query, 
                                        IPersistenceCommands pcommand,
                                        ICredentialHasher hasher)
        {
            ThrowNullArguments(() => query, () => pcommand);

            _query = query;
            _pcommand = pcommand;

            CredentialHasher = hasher ?? new DefaultHasher();
        }
        
        #region ICredentialAuthentication
        /// <summary>
        /// NOTE that this method doesn't enforce that the CURRENT user be the one initiating it, thus, this method should not be
        /// exposed publicly, rather, it should be part of some other business process where the right policy for who can assign
        /// a credential to whom can be enforced.
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public IOperation AssignCredential(Credential credential)
        => LazyOp.Try(() =>
        {
            if (!_query.UserExists(credential.Owner.UserId)) throw new Exception("could not find user");
            else
            {
                //find and Expire any old credentials of the same name and access belonging to the user
                _query.GetCredentials(credential.Owner.UserId, credential.Metadata)
                      .Where(_cred => DateTime.Now > _cred.ExpiresOn)
                      .Select(ExpireCredential)

                      //add the new credential ONLY after all unexpired credentials of the same metadata type have been expired.
                      .Chain()
                      .Then(() => _pcommand.Add(CreateCredential(credential.Owner.UserId, credential.Value, credential.Metadata, credential.ExpiresOn)))
                      .Resolve();
            }
        });

        private Credential CreateCredential(string userId, byte[] value, CredentialMetadata metadata, DateTime? expiresOn) 
        => CreateCredential(0, userId, value, metadata, expiresOn);

        private Credential CreateCredential(long entityId, string userId, byte[] value, CredentialMetadata metadata, DateTime? expiresOn)
        => new Credential
        {
            UniqueId = entityId,
            Owner = new Identity.Principal.User { UniqueId = userId },
            Metadata = metadata,
            Value = metadata.Access == Access.Public ? value : null,
            SecuredHash = metadata.Access == Access.Secret ? CredentialHasher.CalculateHash(value) : null,
            ExpiresOn = expiresOn
        };

        public IOperation ExpireCredential(Credential credential)
        => LazyOp.Try(() =>
        {
            credential.ExpiresOn = DateTime.Now;
            _pcommand.Add(credential)
                     .Resolve();
        });

        public IOperation VerifyCredential(Credential credential)
        => LazyOp.Try(() =>
        {
            var dbcred = _query
                .GetCredentials(credential.Owner.UniqueId, credential.Metadata)
                .FirstOrDefault(_cred => _cred.ExpiresOn > DateTime.Now)
                .ThrowIfNull("could not find Credential");

            if (dbcred.Metadata.Access == Access.Secret &&
               !CredentialHasher.IsValidHash(credential.Value, dbcred.SecuredHash)) throw new Exception("Invalid Credential");

            else if (dbcred.Metadata.Access == Access.Public &&
                    !dbcred.Value.SequenceEqual(credential.Value)) throw new Exception("Invalid Credential");
        });
        #endregion
    }
}