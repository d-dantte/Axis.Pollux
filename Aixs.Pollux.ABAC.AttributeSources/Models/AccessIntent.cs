using Axis.Pollux.ABAC.Auth;
using Axis.Sigma.Core;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.ABAC.AttributeSources.Models
{
    public class AccessIntent
    {
        public IAttribute ResourceDescriptor { get; private set; }
        public IAttribute ActionDescriptor { get; private set; }

        public AccessIntent(string resourceDescriptor, string actionDescriptor)
        {
            ThrowNullArguments(() => resourceDescriptor, () => actionDescriptor);

            ResourceDescriptor = new IntentAuthorizationAttribute
            {
                Name = Constants.IntentAttribute_ResourceDescriptor,
                Type = Luna.Utils.CommonDataType.String,
                Data = resourceDescriptor
            };

            ActionDescriptor = new IntentAuthorizationAttribute
            {
                Name = Constants.IntentAttribute_ActionDescriptor,
                Type = Luna.Utils.CommonDataType.String,
                Data = actionDescriptor
            };
        }

        public AccessIntent(IAttribute resourceDescriptor, IAttribute actionDescriptor)
        {
            ThrowNullArguments(() => resourceDescriptor, () => actionDescriptor);

            ResourceDescriptor = new IntentAuthorizationAttribute(resourceDescriptor)
                .ThrowIf(_att => _att.Name != Constants.IntentAttribute_ResourceDescriptor, "invalid resource descriptor attribute");

            ActionDescriptor = new IntentAuthorizationAttribute(actionDescriptor)
                .ThrowIf(_att => _att.Name != Constants.IntentAttribute_ActionDescriptor, "invalid action descriptor attribute");
        }
    }
}
