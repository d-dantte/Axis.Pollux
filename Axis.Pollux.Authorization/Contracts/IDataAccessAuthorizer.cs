using Axis.Luna.Operation;
using Axis.Pollux.Authorization.Models;

namespace Axis.Pollux.Authorization.Contracts
{
    public interface IDataAccessAuthorizer
    {
        /// <summary>
        /// Attempt to evaluate the data authorization policy that relates to the given data descriptor. Resource attributes added to the execution context include
        /// the <c>IDataAccessDescriptor.DataType</c>, as well as the serialized version of the object itself.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        Operation AuthorizeAccess(IDataAccessDescriptor data);
    }
}
