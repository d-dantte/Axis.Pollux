using Axis.Sigma.Core.Request;
using Axis.Luna;
using Axis.Luna.Operation;

namespace Axis.Pollux.ABAC.Services
{
    public interface IAuthorizer
    {
        IOperation Authorize(IAuthorizationRequest authRequest);
    }
}
