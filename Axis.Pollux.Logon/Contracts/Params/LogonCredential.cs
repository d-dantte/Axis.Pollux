using Axis.Luna.Operation;
using Axis.Pollux.Common.Exceptions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Logon.Contracts.Params
{
    public class LogonCredential: IValidatable
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public Operation Validate()
        => Operation.Try(() =>
        {
            if(string.IsNullOrWhiteSpace(Name)
            || string.IsNullOrWhiteSpace(Value))
                throw new CommonException(ErrorCodes.ModelValidationError);
        });
    }
}
