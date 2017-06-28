namespace Axis.Pollux.Account
{
    public static class Constants
    {
        #region User Status
        public static readonly int UserStatus_InActive = 0;
        public static readonly int UserStatus_Active = 1;
        public static readonly int UserStatus_Blocked = 2;
        #endregion

        #region VerificationContexts
        public static readonly string VerificationContext_CredentialResetPrefix = "Axis.Pollux.Account.VerificationContext[CredentialReset]";
        public static readonly string VerificationContext_UserActivation = "Axis.Pollux.Account.VerificationContext[UserActivation]";
        #endregion
    }
}
