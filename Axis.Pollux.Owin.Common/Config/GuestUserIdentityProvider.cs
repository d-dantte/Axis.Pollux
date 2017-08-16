using Owin;
using System.Linq;
using System.Security.Claims;

using Axis.Luna.Extensions;
using System;
using System.Collections.Generic;

namespace Axis.Pollux.Owin.Services.Config
{
    public static class GuestUserIdentityProvider
    {
        /// <summary>
        /// This middleware MUST be added after the authentication middleware. Its purpose is to add claims to the ClaimsPrincipal object
        /// that represents a "guest" user in the system. A guest user is one that has not been authenticated.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="guestNameProvider"></param>
        /// <returns></returns>
        public static IAppBuilder UseGuestUserIdentityProvider(this IAppBuilder app, Func<IEnumerable<Claim>> guestClaimsFactory)
        {
            return app.Use(async (context, next) =>
            {
                if(context.Authentication.User != null &&
                   string.IsNullOrWhiteSpace(context.Authentication.User.FindFirst(ClaimTypes.Name)?.Value))
                {
                    var cid = context.Authentication.User.Identity.Cast<ClaimsIdentity>();
                    cid.AddClaims(guestClaimsFactory.Invoke());
                }

                await next();
            });
        }
    }
}