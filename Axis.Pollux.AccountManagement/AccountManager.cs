using System;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.AccountManagement.Queries;
using Axis.Jupiter.Kore.Command;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.Authentication.Service;
using Axis.Pollux.Identity.Services;
using Axis.Luna.Extensions;
using System.Linq;
using Axis.Pollux.Authentication;
using Axis.Pollux.Account;
using Axis.Pollux.Account.Objects;

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

        public virtual Operation<User> RegisterUser(string targetUser, int userStatus, Credential secretCredential)
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

        public virtual Operation<ContextVerification> GenerateContextVerification(string targetUser, string verificationContext, DateTime expiryDate)
        => Operation.Try(() =>
        {
            var user = _query.GetUser(targetUser).ThrowIfNull("user not found");
            if (user.Status != Constants.UserStatus_Active) throw new Exception("invalid account state");

            var verification = _query.GetLatestVerification(user.UserId, verificationContext);

            //if no unverified context still exists in the db, create a new one
            if (verification == null ||
                verification.Verified ||
                verification.ExpiresOn <= DateTime.Now) return _pcommand.Add(new ContextVerification
                {
                    Context = verificationContext,
                    ExpiresOn = expiryDate,
                    Target = user,
                    VerificationToken = GenerateToken(),
                    Verified = false
                })
                .Resolve();

            else return verification;
        });

        public virtual Operation<User> ActivateUser(string targetUser)
        => Operation.Try(() =>
        {
            var user = _query.GetUser(targetUser);
            if (user?.Status == Constants.UserStatus_InActive)
            {
                //activate the user
                user.Status = Constants.UserStatus_Active;
                return _pcommand.Update(user);
            }
            else throw new Exception("user not found");
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
        => GenerateContextVerification(targetUser, 
                                       Constants.VerificationContext_CredentialResetPrefix + $"{meta.Access}-{meta.Name}", 
                                       DateTime.Now - validityDuration);

        public Operation<ContextVerification> GenerateUserActivationVerification(string targetUser, TimeSpan validityDuration)
        => GenerateContextVerification(targetUser,
                                       Constants.VerificationContext_UserActivation,
                                       DateTime.Now - validityDuration);


        public virtual Operation ResetCredential(string targetUser, Credential @new, string verificationToken)
        => VerifyContext(targetUser,
                         Constants.VerificationContext_CredentialResetPrefix + $"{@new.Metadata.Access}-{@new.Metadata.Name}",
                         verificationToken)
                        .Then(opr => _credAuth.AssignCredential(targetUser, @new));

        public virtual Operation<long> UserCount()
        => Operation.Try(() => _query.UserCount());

        public virtual Operation VerifyContext(string userId, string verificationContext, string token)
        => Operation.Try(() =>
        {
            var cv = _query.GetContextVerification(userId, verificationContext, token)
                           .ThrowIf(_cv => _cv == null || _cv.Verified, "verification token is invalid");

            cv.Verified = true;
            _pcommand.Update(cv).Resolve();
        });

        public virtual Operation<User> VerifyUserActivation(string targetUser, string verificationToken)
        => VerifyContext(targetUser,
                         Constants.VerificationContext_UserActivation,
                         verificationToken)
                        .Then(opr => ActivateUser(targetUser));


        private string GenerateToken() => RandomAlphaNumericGenerator.RandomAlphaNumeric(50);
    }
}
