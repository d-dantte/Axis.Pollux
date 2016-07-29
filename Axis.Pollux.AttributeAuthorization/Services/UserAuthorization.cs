using static Axis.Luna.Extensions.ExceptionExtensions;

using Axis.Pollux.ABAC.Services;
using Axis.Luna;
using Axis.Sigma.Core.Request;
using Axis.Sigma.Core.Authority;
using Axis.Sigma.Core.Policy;

namespace Axis.Pollux.AttributeAuthorization.Services
{
    public class UserAuthorization : IUserAuthorization
    {
        public Operation Authorize(AuthorizationRequest authRequest)
            => Operation.Run(() =>
            {
                _authority.Authorize(authRequest)
                          .ThrowIf(effect => effect == Effect.Deny, "Access Denied");
            });

        private PolicyAuthority _authority { get; set; }

        public UserAuthorization(AuthorityConfiguration config)
        {
            ThrowNullArguments(() => config);

            _authority = new PolicyAuthority(config);
        }
    }
}
