using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.OperationExtensions;

using System;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.Authentication;
using Axis.Pollux.Authentication.Service;
using Axis.Pollux.CoreAuthentication.Queries;
using Axis.Jupiter.Kore.Command;

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
        public Operation AssignCredential(string userId, Credential credential)
        => Operation.Run(() =>
        {
            if (!_query.UserExists(userId)) throw new Exception("could not find user");
            else
            {
                //find and deactivate any old credentials of the same name and access belonging to the user
                var oldCred = _query.GetCredential(userId, credential.Metadata);                    
                if (oldCred != null) //deactivate
                {
                    oldCred.Status = CredentialStatus.Inactive;
                    _pcommand.Update(oldCred).Resolve();
                }

                _pcommand.Add(CreateCredential(userId, credential.Value, credential.Metadata, credential.ExpiresIn))
                         .Resolve();
            }
        });

        private Credential CreateCredential(string userId, byte[] value, CredentialMetadata metadata, long? expiresIn) => CreateCredential(0, userId, value, metadata, expiresIn);

        private Credential CreateCredential(long entityId, string userId, byte[] value, CredentialMetadata metadata, long? expiresIn)
        => new Credential
        {
            EntityId = entityId,
            OwnerId = userId,
            Metadata = metadata.ThrowIfNull(),
            Value = metadata.Access == Access.Public ? value : null,
            SecuredHash = metadata.Access == Access.Secret ? CredentialHasher.CalculateHash(value) : null,
            ExpiresIn = expiresIn
        };

        public Operation DeleteCredential(Credential credential)
        => Operation.Run(() =>
        {
            _pcommand.Delete(credential).Resolve();
        });

        public Operation VerifyCredential(Credential credential)
        => Operation.Run(() =>
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

        public Operation ModifyCredential(Credential old, Credential @new)
        => VerifyCredential(old).Then(_opr =>
        {
            _pcommand.Update(CreateCredential(old.EntityId, old.OwnerId, @new.Value, old.Metadata, old.ExpiresIn)).Resolve();
        });
        #endregion
    }
}
