using Axis.Sigma.Core.Request;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.ABAC.Auth;

namespace Axis.Pollux.ABAC.Services
{
    public interface IUserAuthorization
    {
        Operation Authorize(AuthorizationRequest authRequest);
    }
}
