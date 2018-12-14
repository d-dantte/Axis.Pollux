using System;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Authentication.Contracts.Params
{
    public class UniqueCredentialValidationInfo : IValidatable
    {
        /// <summary>
        /// Credential Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Credential Value (input format)
        /// </summary>
        public string Data { get; set; }

        public Operation Validate()
        => Operation.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(Name)
                || string.IsNullOrWhiteSpace(Data))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.ModelValidationError);
        });
    }


    public class NonUniqueCredentialValidationInfo : IValidatable
    {
        /// <summary>
        /// Credential Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Credential Value (input format)
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Id of the user to whom the credential belongs
        /// </summary>
        public Guid UserId { get; set; }

        public Operation Validate()
        => Operation.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(Name)
                || string.IsNullOrWhiteSpace(Data)
                || UserId == default(Guid))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.ModelValidationError);
        });
    }
}
