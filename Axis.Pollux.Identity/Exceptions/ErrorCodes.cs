using System;
using System.Collections.Generic;
using System.Text;

namespace Axis.Pollux.Identity.Exceptions
{
    public static class ErrorCodes
    {
        /// <summary>
        /// When the result of a store command is null or invalid
        /// </summary>
        public static readonly string InvalidStoreCommandResult = "Pollux.Identity.Error.0";

        /// <summary>
        /// When the result of a store query is null or invalid
        /// </summary>
        public static readonly string InvalidStoreQueryResult = "Pollux.Identity.Error.1";

        /// <summary>
        /// When a contract params Validate method fails
        /// </summary>
        public static readonly string InvalidContractParamState = "Pollux.Identity.Error.2";

        /// <summary>
        /// When an invalid argument (nulls, etc) is passed into a method
        /// </summary>
        public static readonly string InvalidArgument = "Pollux.Identity.Error.3";
    }
}
