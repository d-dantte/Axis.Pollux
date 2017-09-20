using System;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.AccountManagement.Queries;

using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.Authentication.Services;
using Axis.Pollux.Identity.Services;
using Axis.Luna.Extensions;
using System.Linq;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Account;
using Axis.Pollux.Account.Models;
using Axis.Jupiter.Commands;
using Axis.Luna.Operation;
using Axis.Pollux.Account.Services;
using System.Collections.Generic;

using static Axis.Luna.Extensions.ValidatableExtensions;

namespace Axis.Pollux.AccountManagement.Services
{
    public class AccountManager : IAccountManager
    {
        private IAccountQuery _query;
        private IPersistenceCommands _pcommand;
        private ICredentialAuthority _credAuth;
        private IUserManager _userManager;

        public AccountManager(IAccountQuery query, IPersistenceCommands pcommand,
                              ICredentialAuthority credentialAuth,
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

        /// <summary>
        /// Register a user and assign a default credential
        /// </summary>
        /// <param name="targetUser"></param>
        /// <param name="userStatus"></param>
        /// <param name="secretCredential">this object should have it's Owner set to NULL</param>
        /// <returns></returns>
        public virtual IOperation<User> RegisterUser(string targetUser, int userStatus, Credential secretCredential)
        => ValidateModels(secretCredential)
        .Then(() => _userManager.CreateUser(targetUser, userStatus))
        .Then(user =>
        {
            secretCredential.Owner = user;
            return _credAuth
                .AssignCredential(secretCredential)
                .Then(() => user);
        });

        public virtual IOperation<User> BlockUser(string targetUser)
        => LazyOp.Try(() =>
        {
            var user = _query.GetUser(targetUser).ThrowIfNull("user not found");
            if (user.Status != Constants.UserStatus_Blocked)
            {
                //invalidate all user-logons
                return _query
                    .GetValidUserLogons(targetUser).Page
                    .Select(next =>
                    {
                        next.Invalidated = true;
                        return _pcommand.Update(next);
                    })
                    .UsingEach(_op =>
                    {
                        try
                        {
                            _op.Resolve();
                        }
                        catch { }
                    })
                    .Pipe(_logons =>
                    {
                        user.Status = Constants.UserStatus_Blocked;
                        return _pcommand.Update(user).Resolve();
                    });
            }
            else return user;
        });

        public virtual IOperation<User> DeactivateUser(string targetUser)
        => LazyOp.Try(() =>
        {
            var user = _query.GetUser(targetUser).ThrowIfNull("user not found");
            if (user.Status == Constants.UserStatus_Active)
            {
                //invalidate all user-logons
                return _query.GetValidUserLogons(targetUser).Page
                .Select(next =>
                {
                    next.Invalidated = true;
                    return _pcommand.Update(next);
                })
                .UsingEach(_op =>
                {
                    try
                    {
                        _op.Resolve();
                    }
                    catch { }
                })
                .Pipe(_logons =>
                {
                    user.Status = Constants.UserStatus_Inactive;
                    return _pcommand.Update(user).Resolve();
                });
            }
            else return user;
        });

        public virtual IOperation<User> ActivateUser(string targetUser)
        => LazyOp.Try(() =>
        {
            var user = _query.GetUser(targetUser).ThrowIfNull("user not found");
            if (user.Status == Constants.UserStatus_Inactive ||
                user.Status == Constants.UserStatus_Blocked)
            {
                //activate the user
                user.Status = Constants.UserStatus_Active;
                return _pcommand.Update(user).Resolve();
            }
            else if (user.Status == Constants.UserStatus_Active) return user;
            else throw new Exception("Invalid user state");
        });

        public virtual IOperation<ContextVerification> GenerateContextVerification(string targetUser, string verificationContext, VerificationTokenType tokenType, DateTime expiresOn)
        => LazyOp.Try(() =>
        {
            var user = _query.GetUser(targetUser).ThrowIfNull("user not found");

            var verification = _query.GetLatestVerification(user.UserId, verificationContext);

            //if no unverified context still exists in the db, create a new one
            if (verification == null ||
                verification.Verified ||
                verification.ExpiresOn <= DateTime.Now) return _pcommand.Add(new ContextVerification
            {
                Context = verificationContext,
                ExpiresOn = expiresOn,
                Target = user,
                VerificationToken = GenerateToken(tokenType),
                Verified = false
            })
            .Resolve();

            else return verification;
        });

        public virtual IOperation VerifyContext(string userId, string verificationContext, string token)
        => LazyOp.Try(() =>
        {
            var cv = _query.GetContextVerification(userId, verificationContext, token)
                           .ThrowIf(_cv => _cv == null || _cv.Verified, "verification token is invalid");

            cv.Verified = true;
            _pcommand.Update(cv).Resolve();
        });

        public virtual IOperation<ContextVerification> RequestCredentialResetVerification(string targetUser, CredentialMetadata meta, TimeSpan validityDuration)
        => ValidateModels(meta)
        .Then(() => GenerateContextVerification(targetUser, 
                                                Constants.VerificationContext_CredentialResetPrefix + $".{meta.Access}-{meta.Name}", 
                                                VerificationTokenType.RandomString,
                                                DateTime.Now - validityDuration));

        public virtual IOperation<ContextVerification> RequestUserActivationVerification(string targetUser, TimeSpan validityDuration)
        => GenerateContextVerification(targetUser,
                                       Constants.VerificationContext_UserActivation,
                                       VerificationTokenType.RandomString,
                                       DateTime.Now - validityDuration);


        public virtual IOperation ResetCredential(Credential newCredential, string verificationToken)
        =>  ValidateModels(newCredential)
        .Then(() => VerifyContext(newCredential.Owner.UserId,
                                  Constants.VerificationContext_CredentialResetPrefix + $".{newCredential.Metadata.Access}-{newCredential.Metadata.Name}",
                                  verificationToken))
        .Then(() => _credAuth.AssignCredential(newCredential));

        public virtual IOperation<User> VerifyUserActivation(string targetUser, string verificationToken)
        => VerifyContext(targetUser,
                         Constants.VerificationContext_UserActivation,
                         verificationToken)
        .Then(() => ActivateUser(targetUser));

        public virtual IOperation<long> UserCount()
        => LazyOp.Try(() => _query.UserCount());


        private string GenerateToken(VerificationTokenType tokenType)
        {
            switch (tokenType)
            {
                case VerificationTokenType.RandomString: return RandomAlphaNumericGenerator.RandomAlphaNumeric(50);
                case VerificationTokenType.FourDigit: return RandomAlphaNumericGenerator.RandomNumeric(4);
                default: throw new Exception("invalid token type requested");
            }
        }

        public IOperation<UserLogon> GetUserLogonWithToken(string userId, string token) => LazyOp.Try(() => _query.GetUserLogonWithToken(userId, token));

        public IOperation<IEnumerable<UserLogon>> GetUserLogons(string userId, string ipaddress = null, string location = null, string locale = null, string device = null)
        => LazyOp.Try(() => _query.GetUserLogons(userId, ipaddress, location, locale, device));


        public IOperation<UserLogon> InvalidateUserLogon(string userId, string token)
        => LazyOp.Try(() =>
        {
            var logon = _query
                .GetUserLogonWithToken(userId, token)
                .ThrowIfNull("No logon found for the given user & token");

            logon.Invalidated = true;

            return _pcommand.Update(logon);
        });

        public IOperation<UserLogon> AcquireUserLogon(UserLogon logon)
        => ValidateModels(logon).Then(() =>
        {
            return _pcommand.Update(logon);
        });
    }
}
