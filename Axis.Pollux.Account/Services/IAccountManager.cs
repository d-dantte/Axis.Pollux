using Axis.Luna.Operation;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Authentication;
using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.Account
{
    public interface IAccountManager
    {
        #region Account
        IOperation<User> RegisterUser(string targetUser, int userStatus, Credential secretCredential);

        IOperation<User> DeactivateUser(string targetUser);
        IOperation<User> ActivateUser(string targetUser);

        /// <summary>
        /// Enables an administrator to block a target user - depending on Domain needs
        /// </summary>
        /// <param name="targetUser"></param>
        /// <returns></returns>
        IOperation<User> BlockUser(string targetUser);


        IOperation<ContextVerification> GenerateUserActivationVerification(string targetUser, TimeSpan validityDuration);

        IOperation<User> VerifyUserActivation(string targetUser, string contextToken);


        IOperation<long> UserCount();


        IOperation<ContextVerification> GenrateCredentialResetVerification(string targetUser, CredentialMetadata credentialMetadata, TimeSpan validityDuration);

        IOperation ResetCredential(string targetUser, Credential newCredential, string verificationToken);

        #endregion

        #region Context Verification
        IOperation<ContextVerification> GenerateContextVerification(string userId, string verificationContext, DateTime expiresOn);

        IOperation VerifyContext(string userId, string verificationContext, string token);
        #endregion
    }
}
