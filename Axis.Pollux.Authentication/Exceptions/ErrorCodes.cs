namespace Axis.Pollux.Authentication.Exceptions
{
    public static class ErrorCodes
    {
        /// <summary>
        /// When the supplied authentication parameters do not match any credentials for the specified user
        /// </summary>
        public static readonly string InvalidAuthenticationInfo = "Pollux.Authentication.Error.0";

        /// <summary>
        /// When the result of a store command is null or invalid
        /// </summary>
        public static readonly string InvalidStoreCommandResult = "Pollux.Authentication.Error.1";

        /// <summary>
        /// When the result of a store query is null or invalid
        /// </summary>
        public static readonly string InvalidStoreQueryResult = "Pollux.Authentication.Error.2";

        /// <summary>
        /// When the attempting to add a unique credential and another credential already exists with the specified value
        /// </summary>
        public static readonly string UniqueCredentialViolation = "Pollux.Authentication.Error.3";

        /// <summary>
        /// When StoreProvider.CommandFor(...) returns null
        /// </summary>
        public static readonly string UnmappedStoreCommandModelType = "Pollux.Authentication.Error.4";

        /// <summary>
        /// When a multi-factor authentication is requested and the public token is sent to the user
        /// </summary>
        public static readonly string MultiFactorAuthenticationRequest = "Pollux.Authentication.Error.5";

        /// <summary>
        /// When a search for a multi-factor configuration using an event label turns up null
        /// </summary>
        public static readonly string UnmappedMultiFactorEventLabelConfiguration = "Pollux.Authentication.Error.6";

        /// <summary>
        /// When a multi-factor authentication fails for whatever reason
        /// </summary>
        public static readonly string MultiFactorAuthenticationFailure = "Pollux.Authentication.Error.7";
    }
}
