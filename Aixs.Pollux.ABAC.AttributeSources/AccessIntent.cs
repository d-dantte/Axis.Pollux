using Axis.Pollux.ABAC.Auth;
using Axis.Sigma.Core;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.ABAC.AttributeSources
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
                Name = CommonAttributeNames.IntentAttribute_ResourceDescriptor,
                Type = Luna.CommonDataType.String,
                Data = resourceDescriptor
            };

            ActionDescriptor = new IntentAuthorizationAttribute
            {
                Name = CommonAttributeNames.IntentAttribute_ActionDescriptor,
                Type = Luna.CommonDataType.String,
                Data = actionDescriptor
            };
        }

        public AccessIntent(IAttribute resourceDescriptor, IAttribute actionDescriptor)
        {
            ThrowNullArguments(() => resourceDescriptor, () => actionDescriptor);

            ResourceDescriptor = new IntentAuthorizationAttribute(resourceDescriptor)
                .ThrowIf(_att => _att.Name != CommonAttributeNames.IntentAttribute_ResourceDescriptor, "invalid resource descriptor attribute");

            ActionDescriptor = new IntentAuthorizationAttribute(actionDescriptor)
                .ThrowIf(_att => _att.Name != CommonAttributeNames.IntentAttribute_ActionDescriptor, "invalid action descriptor attribute");
        }
    }
}
