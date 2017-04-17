using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Sigma.Core;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.Services
{
    public interface IIntentAttributeManager
    {
        Operation<IAttribute> AssignAttribute(string intentId, IntentAuthorizationAttribute attribute);
        Operation<IAttribute> AddAttribute(IntentAuthorizationAttribute attribute);

        Operation<IAttribute> UpdateAttribute(IntentAuthorizationAttribute attribute);
        Operation<IAttribute> RemoveAttribute(string attributeName);

        Operation<IEnumerable<IAttribute>> GetAttributes();
        Operation<IEnumerable<IAttribute>> GetAttributesFor(string intentId);
    }
}
