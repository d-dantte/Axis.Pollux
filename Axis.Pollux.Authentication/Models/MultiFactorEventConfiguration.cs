using System;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Authentication.Models
{
    public class MultiFactorEventConfiguration: BaseModel<Guid>
    {
        public TimeSpan ValidityDuration { get; set; }
        public string EventLabel { get; set; }
        public string[] CommunicationChannels { get; set; }

        public override Operation Validate()
        => base.Validate().Then(() =>
        {
            if(ValidityDuration == default(TimeSpan)
               || string.IsNullOrWhiteSpace(EventLabel)
               || CommunicationChannels == null
               || CommunicationChannels.Length == 0)
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.ModelValidationError);
        });
    }
}
