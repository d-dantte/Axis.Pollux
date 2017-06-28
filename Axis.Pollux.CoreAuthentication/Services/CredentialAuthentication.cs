using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;

using System;
using System.Linq;
using Axis.Pollux.Authentication;
using Axis.Pollux.Authentication.Service;
using Axis.Pollux.CoreAuthentication.Queries;
using Axis.Jupiter.Kore.Command;
using Axis.Luna.Operation;

namespace Axis.Pollux.CoreAuthentication.Services
{
    public class CredentialAuthentication: ICredentialAuthentication
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
        public IOperation AssignCredential(Credential credential)
        => LazyOp.Try(() =>
        {
            if (!_query.UserExists(credential.Owner.UserId)) throw new Exception("could not find user");
            else
            {
                //find and Expire any old credentials of the same name and access belonging to the user
                var oldCred = _query.GetCredential(credential.Owner.UserId, credential.Metadata);                    
                if (oldCred != null) //deactivate
                {
                    ExpireCredential(oldCred);
                }

                else
                    _pcommand.Add(CreateCredential(credential.Owner.UserId, credential.Value, credential.Metadata))
                         .Resolve();
            }
        });

        private Credential CreateCredential(string userId, byte[] value, CredentialMetadata metadata) => CreateCredential(0, userId, value, metadata);

        private Credential CreateCredential(long entityId, string userId, byte[] value, CredentialMetadata metadata)
        => new Credential
        {
            UniqueId = entityId,
            Owner = new Identity.Principal.User { Un,
            Metadata = metadata.ThrowIfNull(),
            Value = metadata.Access == Access.Public ? value : null,
            SecuredHash = metadata.Access == Access.Secret ? CredentialHasher.CalculateHash(value) : null,
            ExpiresIn = expiresIn
        };

        public IOperation ExpireCredential(Credential credential)
        => LazyOp.Try(() =>
        {
            throw new NotImplementedException();
        });

        public IOperation VerifyCredential(Credential credential)
        => LazyOp.Try(() =>
        {
            var dbcred = _query
                .GetCredential(credential.OwnerId, credential.Metadata)
                .ThrowIf(_c => _c.Status != CredentialStatus.Active, "credential is not active")
                .ThrowIfNull("could not find Credential");

            if (dbcred.ExpiresIn <= (DateTime.Now - dbcred.CreatedOn).Ticks)
            {
                _pcommand.Update(dbcred.With(new { Status = CredentialStatus.Inactive })).Resolve();
                throw new Exception("Credential has expired");
            }

            if (dbcred.Metadata.Access == Access.Secret &&
               !CredentialHasher.IsValidHash(credential.Value, dbcred.SecuredHash)) throw new Exception("Invalid Credential");

            else if (dbcred.Metadata.Access == Access.Public &&
                    !dbcred.Value.SequenceEqual(credential.Value)) throw new Exception("Invalid Credential");
        });

        public IOperation ModifyCredential(Credential old, Credential @new)
        => VerifyCredential(old).Then(_opr =>
        {
            _pcommand.Update(CreateCredential(old.UniqueId, old.OwnerId, @new.Value, old.Metadata, old.ExpiresIn)).Resolve();
        });
        #endregion
    }
}
