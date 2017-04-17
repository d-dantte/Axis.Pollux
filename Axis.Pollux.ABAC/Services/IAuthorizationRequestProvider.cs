using Axis.Sigma.Core.Request;

namespace Axis.Pollux.ABAC.Services
{
    public interface IAuthorizationRequestProvider
    {
        IAuthorizationRequest CurrentContexRequest();
    }
}
