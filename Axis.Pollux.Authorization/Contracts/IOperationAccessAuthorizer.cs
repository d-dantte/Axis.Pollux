using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Models;

namespace Axis.Pollux.Authorization.Contracts
{
    public interface IOperationAccessAuthorizer
    {
        /// <summary>
        /// Attempts to ascertain if the user found in IUserContext.CurrentUserId() has the permission to access the ContractOperation described byt the "Operation Descriptor" object.
        /// </summary>
        /// <param name="descriptor">object describing the operation being accessed</param>
        /// <returns></returns>
        Operation AuthorizeAccess(OperationAccessDescriptor descriptor);
    }
}
