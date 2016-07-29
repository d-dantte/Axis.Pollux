using Axis.Sigma.Core.Request;
using System.Linq;
using Axis.Luna;
using Axis.Pollux.ABAC.Auth;

namespace Axis.Pollux.ABAC.Services
{
    public interface IUserAuthorization
    {
        Operation AssignUser(string user, AuthorizationAttribute attribute);
        Operation AssignAction(string action, AuthorizationAttribute attribute);
        Operation AssignResource(string resource, AuthorizationAttribute attribute);
        Operation AssignEnvironment(string environment, AuthorizationAttribute attribute);

        Operation<AuthorizationAttribute> CreateAttribute(Category category, string name, object value);
        Operation DeleteAttribute(string owner, AuthorizationAttribute attribute);

        Operation Authorize(AuthorizationRequest authRequest);
    }
}
