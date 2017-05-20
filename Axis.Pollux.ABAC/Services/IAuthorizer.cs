using Axis.Sigma.Core.Request;
using Axis.Luna;

namespace Axis.Pollux.ABAC.Services
{
    public interface IAuthorizer
    {
        Operation Authorize(IAuthorizationRequest authRequest);
    }
}
