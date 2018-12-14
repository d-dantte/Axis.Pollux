namespace Axis.Pollux.Common.Exceptions
{
    public static class ErrorCodes
    {
        /// <summary>
        /// When the result of a store command is null or invalid
        /// </summary>
        public static readonly string ModelValidationError = "Pollux.Common.Error.0";

        /// <summary>
        /// When a contract params Validate method fails
        /// </summary>
        public static readonly string InvalidContractParamState = "Pollux.Common.Error.1";

        /// <summary>
        /// When an invalid argument (nulls, etc) is passed into a method
        /// </summary>
        public static readonly string InvalidArgument = "Pollux.Common.Error.2";
    }
}
