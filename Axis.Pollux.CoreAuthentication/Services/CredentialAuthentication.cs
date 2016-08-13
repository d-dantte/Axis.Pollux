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
                if (!_context.Store<User>().Query.Any(_user => _user.UserId == userId)) throw new Exception("could not find user");
                else
                {
                    _context.Store<Credential>().Add(CreateCredential(userId, credential.Value, credential.Metadata));
                    _context.CommitChanges();
                }
            });

        private Credential CreateCredential(string userId, byte[] value, CredentialMetadata metadata)
            => new Credential
            {
                OwnerId = userId,
                Metadata = metadata.ThrowIfNull(),
                Value = metadata.Access == Access.Public ? value : null,
                SecuredHash = metadata.Access == Access.Secret ? CredentialHasher.CalculateHash(value) : null
            };

        public Operation DeleteCredential(Credential credential)
            => Operation.Run(() =>
            {
                _context.Store<Credential>().Delete(credential, true);
            });

        public Operation VerifyCredential(Credential credential)
            => Operation.Run(() =>
            {
                var dbcred = _context.Store<Credential>().Query
                                     .Where(c => c.OwnerId == credential.OwnerId)
                                     .Where(c => c.Metadata.Name == credential.Metadata.Name)
                                     .FirstOrDefault()
                                     .ThrowIfNull("could not find Credential");

                if (dbcred.Metadata.Access == Access.Secret &&
                   !CredentialHasher.IsValidHash(credential.Value, dbcred.SecuredHash)) throw new Exception("Invalid Credential");

                else if (dbcred.Metadata.Access == Access.Public &&
                        !dbcred.Value.SequenceEqual(credential.Value)) throw new Exception("Invalid Credential");
            });

        public Operation ModifyCredential(Credential oldValue, byte[] newValue)
            => Operation.Run(() =>
            {
                var store = _context.Store<Credential>();
                return this.VerifyCredential(oldValue)
                           .Then(_opr => store.Query
                                              .Where(_cred => _cred.Metadata.Name == oldValue.Metadata.Name)
                                              .Where(_cred => _cred.OwnerId == oldValue.OwnerId)
                                              .FirstOrDefault()
                                              .ThrowIfNull("could not find credential")
                                              .Do(_cred => store.Modify(_cred.With(new { Value = oldValue.Value }), true)));
            });
        #endregion
    }
}
