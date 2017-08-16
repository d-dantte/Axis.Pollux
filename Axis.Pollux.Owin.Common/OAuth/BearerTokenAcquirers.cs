using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Microsoft.Owin.Security.OAuth;
using System;

namespace Axis.Pollux.Owin.Services.OAuth
{
    public static class BearerTokenAcquirers
    {
        public static Func<OAuthRequestTokenContext, AsyncOperation> CookieAcquirer => _cookieAcquirer;
        private static readonly Func<OAuthRequestTokenContext, AsyncOperation> _cookieAcquirer = context =>
        AsyncOp.Try(() =>
        {
            context.ThrowIfNull("invalid context");

            // try to find bearer token in a cookie 
            var tokenCookie = context.OwinContext.Request.Cookies["BearerToken"];

            if (!string.IsNullOrWhiteSpace(tokenCookie)) context.Token = tokenCookie;
            else throw new Exception();
        });
    }
}