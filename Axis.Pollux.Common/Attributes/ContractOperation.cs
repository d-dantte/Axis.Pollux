using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Exceptions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Common.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class ContractOperation: Attribute, IValidatable
    {
        /// <inheritdoc />
        /// <summary>
        /// Creates a new ContractOperation attribute
        /// </summary>
        /// <param name="name">the name given to the contract operation</param>
        public ContractOperation(string name)
        {
            Name = name;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates a new ContractOperation attribute.
        /// </summary>
        public ContractOperation()
        {
            Name = null;
        }

        public string Name { get; }


        public Operation Validate()
        => Operation.Try(() =>
        {
            if(string.IsNullOrWhiteSpace(Name))
                throw new CommonException(ErrorCodes.ModelValidationError);
        });
    }
}
