using System;

namespace Axis.Pollux.Common.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class ContractOperation: Attribute
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
        /// Creates a new ContractOperation attribute. A default name is given to
        /// this contract operation, formed from the namespace+classname+methodname+randomguid
        /// </summary>
        public ContractOperation()
        {
            Name = null;
        }

        public string Name { get; }
    }
}
