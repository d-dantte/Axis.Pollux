using Axis.Luna.Operation;
using Axis.Pollux.ABAC.Auth;
using Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.Services
{
    public interface IIntentAttributeManager
    {
        IOperation<IAttribute> AssignAttribute(string intentId, IntentAuthorizationAttribute attribute);
        IOperation<IAttribute> RemoveAttribute(string intentId, string attributeName);
    }
}
