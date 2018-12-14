using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Communication.Models
{
    public class MessageRef: IValidatable
    {
        public Guid MessageId { get; set; }
        public string ContactChannel { get; set; }
        public string TransportType { get; set; }

        public Operation Validate()
        => Operation.Try(() =>
        {
            if (MessageId == default(Guid)
                || string.IsNullOrWhiteSpace(ContactChannel))
                throw new Exceptions.CommunicationException(Common.Exceptions.ErrorCodes.ModelValidationError);
        });
    }
}
