using System;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Common.Exceptions;
using Axis.Pollux.Common.Models;

using static Axis.Luna.Extensions.ExceptionExtension;

namespace Axis.Pollux.Authentication.Contracts.Params
{
    public enum MultiFactorStep
    {
        Request,
        Response
    }

    public class MultiFactorAuthenticationInfo : IValidatable
    {
        public Guid UserId { get; set; }
        public string CredentialKey { get; set; }
        public string CredentialToken { get; set; }
        public string EventLabel { get; set; }
        public MultiFactorStep Step { get; }

        public string Channel { get; set; }
        public Guid? ContactDataId { get; set; }

        public MultiFactorAuthenticationInfo()
        {
            Step = MultiFactorStep.Request;
        }

        public MultiFactorAuthenticationInfo(MultiFactorStep step)
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
                        || string.IsNullOrWhiteSpace(EventLabel))
                        throw new CommonException(Common.Exceptions.ErrorCodes.ModelValidationError);

                    if (string.IsNullOrWhiteSpace(Channel)
                        && (ContactDataId == null || ContactDataId == default(Guid)))
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
    public class MultiFactorAuthenticationToken: IValidatable
    {
        public Guid UserId { get; set; }
        public string EventLabel { get; set; }
        public string CredentialKey { get; set; }
        public string[] AllowedCommunicationChannels { get; set; }

        public Operation Validate()
        => Operation.Try(() =>
        {
            if (UserId == default(Guid) || AllowedCommunicationChannels == null)
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidContractParamState);

            else
                AllowedCommunicationChannels
                    .Concat(new[]{CredentialKey, EventLabel})
                    .Any(string.IsNullOrWhiteSpace)
                    .ThrowIf(true, new AuthenticationException(Common.Exceptions.ErrorCodes.InvalidContractParamState));
        });
    }
}
