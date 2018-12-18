using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Exceptions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Logon.Contracts.Params
{
    public class LogonResult: IValidatable
    {
        public Guid UserId { get; set; }

        public string SessionToken { get; set; }
        public string TokenType { get; set; }
        public DateTimeOffset? ExpiresOn { get; set; }

        public Operation Validate()
        => Operation.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(SessionToken)
                || string.IsNullOrWhiteSpace(TokenType)
                || UserId == default(Guid))
                throw new CommonException(ErrorCodes.ModelValidationError);
        });
    }
}
