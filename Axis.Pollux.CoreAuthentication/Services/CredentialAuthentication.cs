using static Axis.Luna.Extensions.ExceptionExtensions;
using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.OperationExtensions;

using System;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.Authentication;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Authentication.Service;

namespace Axis.Pollux.CoreAuthentication.Services
{
    public class CredentialAuthentication: ICredentialAuthentication
    {
        private Jupiter.IDataContext _context = null;
        public ICredentialHasher CredentialHasher { get; private set; }

        //public CredentialAuthentication(Jupiter.IDataContext dataContext) : this(dataContext, null)
        //{ }
        public CredentialAuthentication(Jupiter.IDataContext dataContext, ICredentialHasher hasher)
        {
            ThrowNullArguments(() => dataContext);

            _context = dataContext;
            CredentialHasher = hasher ?? new DefaultHasher();
        }

        #region ICredentialAuthentication
        public Operation AssignCredential(string userId, Credential credential)
            => Operation.Run(() =>
            {
                if (!_context.Store<User>().Query.Any(_user => _user.EntityId == userId)) throw new Exception("could not find user");
                else
                {
                    var credStore = _context.Store<Credential>();

                    //find and deactivate any old credentials of the same name and access belonging to the user
                    var oldCred = credStore.Query
                        .Where(_cred => _cred.Metadata.Name == credential.Metadata.Name)
                        .Where(_cred => _cred.Metadata.Access == credential.Metadata.Access)
                        .Where(_cred => _cred.OwnerId == userId)
                        .OrderByDescending(_cred => _cred.CreatedOn)
                        .FirstOrDefault();                    
                    if (oldCred != null) //deactivate
                    {
                        oldCred.Status = CredentialStatus.Inactive;
                        credStore.Modify(oldCred);
                    }

                    credStore.Add(CreateCredential(userId, credential.Value, credential.Metadata, credential.ExpiresIn));
                    _context.CommitChanges();
                }
            });

        private Credential CreateCredential(string userId, byte[] value, CredentialMetadata metadata, long? expiresIn)
            => new Credential
            {
                OwnerId = userId,
                Metadata = metadata.ThrowIfNull(),
                Value = metadata.Access == Access.Public ? value : null,
                SecuredHash = metadata.Access == Access.Secret ? CredentialHasher.CalculateHash(value) : null,
                ExpiresIn = expiresIn
            };

        public Operation DeleteCredential(Credential credential)
            => Operation.Run(() =>
            {
                _context.Store<Credential>().Delete(credential, true);
            });

        public Operation VerifyCredential(Credential credential)
            => Operation.Run(() =>
            {
                var credstsore = _context.Store<Credential>();
                var dbcred = credstsore.Query
                    .Where(c => c.OwnerId == credential.OwnerId)
                    .Where(c => c.Metadata.Name == credential.Metadata.Name)
                    .Where(c => c.Status == CredentialStatus.Active)
                    .OrderByDescending(c => c.CreatedOn)
                    .FirstOrDefault()
                    .ThrowIfNull("could not find Credential");

                if (dbcred.ExpiresIn <= (DateTime.Now - dbcred.CreatedOn).Ticks)
                {
                    credstsore.Modify(dbcred.With(new { Status = CredentialStatus.Inactive })).Context.CommitChanges();
                    throw new Exception("Credential has expired");
                }

                if (dbcred.Metadata.Access == Access.Secret &&
                   !CredentialHasher.IsValidHash(credential.Value, dbcred.SecuredHash)) throw new Exception("Invalid Credential");

                else if (dbcred.Metadata.Access == Access.Public &&
                        !dbcred.Value.SequenceEqual(credential.Value)) throw new Exception("Invalid Credential");
            });

        public Operation ModifyCredential(Credential modifiedCredential)
            => Operation.Run(() =>
            {
                var store = _context.Store<Credential>();
                return this.VerifyCredential(modifiedCredential)
                           .Then(_opr =>
                           {
                               store.Modify(modifiedCredential, true);
                           });
            });
        #endregion
    }
}
