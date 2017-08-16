using Axis.Pollux.Identity.Services;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.ABAC.Auth;
using System.Linq;
using Axis.Luna.Extensions;

using static Axis.Luna.Extensions.ExceptionExtensions;
using System.Security.Claims;
using System;

namespace Axis.Pollux.Owin.Services.Misc
{
    public class AuthenticatedUserContext : IUserContext
    {
        private AuthorizablePrincipal _user;
        private IOwinContextProvider _owinProvider;

        public AuthenticatedUserContext(IOwinContextProvider owinProvider)
        {
            ThrowNullArguments(() => owinProvider);

            _owinProvider = owinProvider;
        }

        public User User() => _user ?? (_user = AcquireUser());

        /// <summary>
        /// Get the user name, status (if present), and roles from the claims principal
        /// </summary>
        /// <returns></returns>
        private AuthorizablePrincipal AcquireUser()
        {
            var _claimsPrincipal = _owinProvider.Owin().Resolve().Authentication.User;
            _user = new AuthorizablePrincipal
            {
                Status = _claimsPrincipal.Claims
                    .FirstOrDefault(_c => _c.Type == ABAC.Constants.SubjectAttribute_UserStatus)
                    .Pipe(_s => _s?.Value ?? "0")
                    .Pipe(int.Parse),

                UserId = _claimsPrincipal.Claims
                    .First(_c => _c.Type == ClaimTypes.Name).Value,

                UId = _claimsPrincipal.Claims
                    .FirstOrDefault(_c => _c.Type == ClaimTypes.Sid)
                    .Pipe(_s => _s?.Value ?? "00000000-0000-0000-0000-000000000000") //<-- empty GUID
                    .Pipe(Guid.Parse)
            };

            _user.SubjectAttributes = new[]
            {
                new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.Integer,
                    Name = ABAC.Constants.SubjectAttribute_UserStatus,
                    Data = _user.Status.ToString()
                },
                new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.String,
                    Name = ABAC.Constants.SubjectAttribute_UserName,
                    Data = _user.UserId
                },
                new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.Guid,
                    Name = ABAC.Constants.SubjectAttribute_UserUUID,
                    Data = _user.UId.ToString()
                }
            }
            .Concat(_claimsPrincipal.Claims.Where(_c => _c.Type == ClaimTypes.Role).Select(_c =>
            {
                return new SubjectAuthorizationAttribute
                {
                    Type = Luna.Utils.CommonDataType.String,
                    Name = ABAC.Constants.SubjectAttribute_UserRole,
                    Data = _c.Value
                };
            }));

            return _user;
        }
    }
}