using Axis.Luna;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Authentication;
using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.Account
{
    public interface IAccountManager
    {
        #region Account
        Operation<User> RegisterUser(string targetUser, int userStatus, Credential secretCredential);

        Operation<User> DeactivateUser(string targetUser);

        /// <summary>
        /// Enables an administrator (or @root) to block a target user
        /// </summary>
        /// <param name="targetUser"></param>
        /// <returns></returns>
        Operation<User> BlockUser(string targetUser);


        Operation<ContextVerification> GenerateUserActivationVerification(string targetUser, TimeSpan validityDuration);

        Operation<User> VerifyUserActivation(string targetUser, string contextToken);


        Operation<long> UserCount();


        Operation<ContextVerification> GenrateCredentialResetVerification(string targetUser, CredentialMetadata credentialMetadata, TimeSpan validityDuration);

        Operation ResetCredential(Credential newCredential, string verificationToken, string targetUser);

        #endregion

        #region Context Verification
        Operation<ContextVerification> CreateVerificationObject(string userId, string verificationContext, DateTime expiryDate);

        Operation VerifyContext(string userId, string verificationContext, string token);
        #endregion
    }
}
