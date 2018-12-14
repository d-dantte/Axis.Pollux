using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Exceptions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Authentication.Contracts.Params
{
    public enum MultiFactorStep
    {
        Request,
        Response
    }

    public class MultiFactorValidationInfo : IValidatable
    {
        public Guid UserId { get; set; }
        public string CredentialKey { get; set; }
        public string CredentialToken { get; set; }
        public string EventLabel { get; set; }
        public MultiFactorStep Step { get; }
        public string Channel { get; set; }

        public MultiFactorValidationInfo()
        {
            Step = MultiFactorStep.Request;
        }

        public MultiFactorValidationInfo(MultiFactorStep step)
        {
            Step = step;
        }


        public Operation Validate()
        => Operation.Try(() =>
        {
            switch (Step)
            {
                case MultiFactorStep.Request:
                    if(string.IsNullOrWhiteSpace(EventLabel))
                        throw new CommonException(Common.Exceptions.ErrorCodes.ModelValidationError);
                    break;

                case MultiFactorStep.Response:
                    if (UserId == default(Guid)
                        || string.IsNullOrWhiteSpace(CredentialKey)
                        || string.IsNullOrWhiteSpace(CredentialToken)
                        || string.IsNullOrWhiteSpace(Channel)
                        || string.IsNullOrWhiteSpace(EventLabel))
                        throw new CommonException(Common.Exceptions.ErrorCodes.ModelValidationError);
                    break;

                default:
                    throw new CommonException(Common.Exceptions.ErrorCodes.ModelValidationError);
            }
        });
    }

    /// <summary>
    /// This data is fed into the exception that is raised when a multi-factor authentication is needed
    /// </summary>
    public class MultiFactorRequestErrorData
    {
        public Guid UserId { get; set; }
        public string EventLabel { get; set; }
        public string CredentialKey { get; set; }
        public string[] AllowedCommunicationChannels { get; set; }
    }
}
