using Axis.Luna.Operation;
using Axis.Sigma;

namespace Axis.Pollux.Authorization.Abac.Contracts
{
    /// <summary>
    /// An implementation is provided by the consuming application. The implementation is responsible for pulling together
    /// all information relevant for authorization as it deems fit. Pollux.Authorization.Abac is in turn responsible for
    /// providing the resource attributes that identify the resources being queried (Operation access, Data access).
    /// </summary>
    public interface IAuthorizationContextProvider
    {
        Operation<IAuthorizationContext> ExecutionAuthorizationContext(params IAttribute[] additionalAttributes);
    }
}
