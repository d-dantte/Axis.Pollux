using System;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Exceptions;
using Axis.Pollux.Communication.Models;

namespace Axis.Pollux.Authentication.Services.CommsMessages
{
    public class MultiFactorAuthenticationRequestMessage: IMessagePayload
    {
        public string PayloadType { get; set; }
        public string Token { get; set; }
        public string Key { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }


        public Operation Validate()
        => Operation.Try(() =>
        {
            var values = new[] {PayloadType, Token, Key};
            if (ExpiresOn == default(DateTimeOffset)
                || values.Any(string.IsNullOrWhiteSpace))
                throw new AuthenticationException(Common.Exceptions.ErrorCodes.ModelValidationError);
        });
    }
}
