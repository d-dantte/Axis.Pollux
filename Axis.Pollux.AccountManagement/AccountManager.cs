using System;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Account;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Account.Queries;
using Axis.Jupiter.Kore.Command;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.Authentication.Service;
using Axis.Pollux.Identity.Services;
using Axis.Luna.Extensions;
using System.Linq;
using Axis.Pollux.Authentication;

namespace Axis.Pollux.AccountManagement
{
    public class AccountManager : IAccountManager
    {
        private IAccountQuery _query;
        private IPersistenceCommands _pcommand;
        private ICredentialAuthentication _credAuth;
        private IUserManager _userManager;

        public AccountManager(IAccountQuery query, IPersistenceCommands pcommand,
                              ICredentialAuthentication credentialAuth,
                              IUserManager userManager)
        {
            ThrowNullArguments(() => query,
                               () => pcommand,
                               () => credentialAuth,
                               () => userManager);

            _query = query;
            _pcommand = pcommand;
            _credAuth = credentialAuth;
            _userManager = userManager;
        }

        public virtual Operation<User> RegisterUser(string targetUser, int userStatus, Authentication.Credential secretCredential)
        => _userManager.CreateUser(targetUser, userStatus)
                       .Then(opr => opr.Result.UsingValue(u => _credAuth.AssignCredential(u.UserId, secretCredential)));

        public virtual Operation<User> BlockUser(string targetUser)
        => Operation.Try(() =>
        {
            var user = _query.GetUser(targetUser).ThrowIfNull("user not found");
            if (user.Status != Constants.UserStatus_Blocked)
            {
                user.Status = Constants.UserStatus_Blocked;
                _pcommand.Update(user);
            }

            return user;
        });

        public virtual Operation<ContextVerification> CreateVerificationObject(string userId, string verificationContext, DateTime expiryDate)
        => Operation.Try(() =>
        {
            var user = _query.GetUser(userId).ThrowIfNull("user not found");
            var _cv = new ContextVerification
            {
                Context = verificationContext,
                ExpiresOn = expiryDate,
                Target = user,
                VerificationToken = GenerateToken(),
                Verified = false
            };

            return _pcommand.Add(_cv);
        });

        public virtual Operation<User> DeactivateUser(string targetUser)
        => Operation.Try(() =>
        {
            var user = _query.GetUser(targetUser);
            if (user?.Status == Constants.UserStatus_Active)
            {
                //invalidate all userlogons
                _query.GetValidUserLogons(targetUser, -1)
                .Page
                .ForAll((cnt, next) =>
                {
                    next.Invalidated = true;
                    _pcommand.Update(next);
                });

                //deactivate the user
                user.Status = Constants.UserStatus_InActive;
                return _pcommand.Update(user);
            }
            else throw new Exception("user not found");
        });

        public virtual Operation<ContextVerification> GenrateCredentialResetVerification(string targetUser, CredentialMetadata meta, TimeSpan validityDuration)
        => Operation.Try(() =>
        {
            var user = _query.GetUser(targetUser);
            if (user.Status != Constants.UserStatus_Active) throw new Exception("invalid account state");

            var context = Constants.VerificationContext_CredentialResetPrefix + $"{meta.Access}-{meta.Name}";
            var verification = _query.GetLatestVerification(user.UserId, context);

            //if no unverified context still exists in the db, create a new one
            if (verification == null || verification.Verified || verification.ExpiresOn <= DateTime.Now)
                return CreateVerificationObject(targetUser, context, DateTime.Now - validityDuration).Resolve();

            else return verification;
        });

        public Operation<ContextVerification> GenerateUserActivationVerification(string targetUser, TimeSpan validityDuration)
        {
            throw new NotImplementedException();
        }


        public virtual Operation ResetCredential(Credential newCredential, string verificationToken, string targetUser)
        {
            throw new NotImplementedException();
        }

        public virtual Operation<long> UserCount()
        {
            throw new NotImplementedException();
        }

        public virtual Operation VerifyContext(string userId, string verificationContext, string token)
        {
            throw new NotImplementedException();
        }

        public virtual Operation<User> VerifyUserActivation(string targetUser, string contextToken)
        {
            throw new NotImplementedException();
        }


        private string GenerateToken() => RandomAlphaNumericGenerator.RandomAlphaNumeric(50);
    }
}
