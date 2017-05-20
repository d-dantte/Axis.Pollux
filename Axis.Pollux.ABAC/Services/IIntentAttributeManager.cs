using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.Services
{
    public interface IIntentAttributeManager
    {
        Operation<IAttribute> AssignAttribute(string intentId, IntentAuthorizationAttribute attribute);
        Operation<IAttribute> RemoveAttribute(string intentId, string attributeName);
    }
}
