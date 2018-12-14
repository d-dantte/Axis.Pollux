using Axis.Luna.Operation;
using Axis.Pollux.Common.Exceptions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Common.Contracts.Params
{
    public class KeyPair: IValidatable
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public Operation Validate()
        => Operation.Try(()=>
        {
            if (string.IsNullOrWhiteSpace(PublicKey))
                throw new CommonException(ErrorCodes.ModelValidationError);

            if (string.IsNullOrWhiteSpace(PrivateKey))
                throw new CommonException(ErrorCodes.ModelValidationError);
        });
    }
}
