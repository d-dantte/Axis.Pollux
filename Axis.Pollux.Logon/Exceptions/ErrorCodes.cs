namespace Axis.Pollux.Logon.Exceptions
{
    public static class ErrorCodes
    {

        /// <summary>
        /// Thrown when any the logon credentials cannot be validated
        /// </summary>
        public static readonly string CredentialValidationError = "Pollux.Logon.Error.0";

        /// <summary>
        /// Thrown when an attempt to modify the status of a logon object (e.g to invalidate it) fails.
        /// </summary>
        public static readonly string InvalidLogonState = "Pollux.Logon.1";
    }
}
