using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.ABAC.AttributeSources.Models;
using Axis.Pollux.ABAC.AttributeSources.Services;
using Axis.Pollux.Identity.Services;
using Axis.Proteus;
using Axis.Sigma.Core.Authority;
using System.Linq;
using System.Reflection;

namespace Axis.Pollux.ABAC.AttributeSources
{
    public static class CommonAuthorizers
    {
        public static IOperation PrincipalSourceOperationIntentAuthorizer(IServiceResolver resolver, MethodInfo operation)
        => LazyOp.Try(() =>
        {
            var authorizableUser = resolver
                .Resolve<IUserContext>()
                .User()
                .Cast<AuthorizablePrincipal>()
                .ThrowIfNull("Non-Authorizable user found in this context");

            var intentSource = resolver
                .Resolve<OperationIntentMap>()
                .Pipe(_oim => new IntentMapSource(_oim, operation));

            var policyAuthority = resolver.Resolve<PolicyAuthority>();

            var authContext = new AuthorizationContext(authorizableUser, 
                                                       intentSource, 
                                                       new EmptyAttributeSource()); //<-- empty for now. stuff like time of day, locale, language, also, some user-logon info could come here too

            return policyAuthority.Authorize(authContext);
        });
    }
}