using Axis.Luna.Operation;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Axis.Luna.Extensions;

namespace Axis.Pollux.Owin.Services.Impl
{
    public class AggregateBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        private List<Func<OAuthRequestTokenContext, AsyncOperation>> _tokenAcquirers = new List<Func<OAuthRequestTokenContext, AsyncOperation>>();
        private List<Func<OAuthValidateIdentityContext, AsyncOperation>> _identityValidators = new List<Func<OAuthValidateIdentityContext, AsyncOperation>>();

        public AggregateBearerAuthenticationProvider AddTokenAcquirer(Func<OAuthRequestTokenContext, AsyncOperation> acquirer)
        {
            _tokenAcquirers.Add(acquirer);
            return this;
        }
        public AggregateBearerAuthenticationProvider AddIdentityValidator(Func<OAuthValidateIdentityContext, AsyncOperation> validator)
        {
            _identityValidators.Add(validator);
            return this;
        }

        public override async Task RequestToken(OAuthRequestTokenContext context)
        {
            await _tokenAcquirers
                .Aggregate(AsyncOp.Try(() => { }), (_op, _acq) => _op.Then(() => _acq.Invoke(context)) as AsyncOperation)
                .Then(() => base.RequestToken(context))
                .Cast<AsyncOperation>();
        }

        public override async Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            await _identityValidators
                .Aggregate(AsyncOp.Try(() => { }), (_op, _vid) => _op.Then(() => _vid.Invoke(context)) as AsyncOperation)
                .Then(() => base.ValidateIdentity(context))
                .Cast<AsyncOperation>();
        }
    }
}
