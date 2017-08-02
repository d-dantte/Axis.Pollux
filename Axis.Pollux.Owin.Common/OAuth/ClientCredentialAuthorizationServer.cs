using Axis.Jupiter.Commands;
using Axis.Luna;
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Pollux.Account.Models;
using Axis.Pollux.Account.Services;
using Axis.Pollux.AccountManagement.Queries;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Authentication.Services;
using Axis.Pollux.Identity.Principal;
using Axis.Pollux.Identity.Services.Queries;
using Axis.Pollux.RoleManagement.Queries;
using Axis.Proteus;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UAParser;
using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.Owin.Services.OAuth
{
    /// <summary>
    /// This server depends on Axis.Account, Axis.Identity, Axis.Authentication, and Axis.RoleManagement modules.
    /// 
    /// I will explore the option of depending ONLY on the IServiceResolver service here, and then subsequently create any service i need, when i need it.
    /// This is because this class is a singleton class, and the services are usually registered as per-request. So its either i create a new ServiceResolver
    /// specifically for this server, or i resolve them when i need them.
    /// </summary>
    public class ClientCredentialAuthorizationServer : OAuthAuthorizationServerProvider, IDisposable//, Microsoft.Owin.Security.Infrastructure.IAuthenticationTokenProvider
    {
        public static readonly string OAuthCustomHeaders_OldToken = "AxisPollux-OldOAuthToken";

        private WeakCache _userLogonCache = null;
        private Parser Parser = Parser.GetDefault();
        private ICredentialAuthority _credAuthority;
        private IAccountManager _accountManager;
        private IPersistenceCommands _pcommands;
        private IUserQuery _userQuery;
        private IRoleManagementQueries _roleQuery;
        private IServiceResolver _resolver;

        public ClientCredentialAuthorizationServer(ICredentialAuthority credentialAuthority, IAccountManager accountManager,
                                                   IPersistenceCommands pcommands, IUserQuery userQuery, IRoleManagementQueries roleQuery,
                                                   IAccountQuery accountQuery, IServiceResolver resolver,
                                                   WeakCache userLogonCache)
        {
            ThrowNullArguments(() => userLogonCache, 
                               () => credentialAuthority,
                               () => accountManager,
                               () => pcommands,
                               () => userQuery,
                               () => roleQuery,
                               () => resolver);

            _userLogonCache = userLogonCache;
            _credAuthority = credentialAuthority;
            _accountManager = accountManager;
            _pcommands = pcommands;
            _userQuery = userQuery;
            _roleQuery = roleQuery;
            _resolver = resolver;
        }

        #region OAuthAuthrizationServerProvider
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        => Task.Run(() =>
        {
            #region invalidate old logons if they exist
            LazyOp.Try(() =>
            {
                var oldToken = context.Request.Headers.GetValues(OAuthCustomHeaders_OldToken)?.FirstOrDefault() ?? null;
                if (oldToken != null)
                {
                    var logon = _userLogonCache.GetOrRefresh<UserLogon>(oldToken);
                    if (logon != null) _accountManager.InvalidateUserLogon(logon.User.UserId, logon.SecurityToken).Resolve();
                }
            })
            #endregion

            #region Verify the user exists
            .Then(() =>
            {
                return _userQuery
                    .GetUserById(context.UserName)
                    .ThrowIfNull("invalid user credential");
            })
            #endregion

            #region verify credentials with the credential authority
            .Then(_user =>
            {
                _credAuthority.VerifyCredential(new Credential
                {
                    Owner = _user,
                    Metadata = CredentialMetadata.Password,
                    Value = Encoding.UTF8.GetBytes(context.Password)
                })
                .Resolve();
                return _user;
            })
            #endregion

            #region aggregate the claims that makeup the token
            .Then(_user =>
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Sid, _user.UId.ToString()));
                identity.AddClaim(new Claim(ABAC.Constants.SubjectAttribute_UserStatus, _user.Status.ToString()));

                //roles
                _roleQuery
                    .GetUserRolesFor(context.UserName).Page
                    .ForAll((_cnt, _next) => identity.AddClaim(new Claim(ClaimTypes.Role, _next.Role.RoleName)));

                return identity.ValuePair(_user);
            })
            #endregion

            #region  extended verification
            .Then(_pair => ExtendedUserVerification(_pair.Key, _pair.Value, _resolver))
            #endregion

            #region Finally
            .Then(_identity => context.Validated(new Microsoft.Owin.Security.AuthenticationTicket(_identity, null)),
            #endregion
            
            #region if any of the above failed...
            err =>
            {
                context.SetError("invalid_grant", err.Message);
                context.Rejected();
            })
            #endregion

            .Resolve();
        });

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        => Task.Run(() =>
        {
            //cache the logon associated to the given token
            _userLogonCache.GetOrAdd(context.AccessToken, _token =>
            {
                var agent = Parser.Parse(context.Request.Headers.Get("User-Agent"));

                var _logon = _accountManager
                    .GetUserLogonWithToken(context.Identity.Name, _token)
                    .Resolve();

                if (_logon != null) return _logon;
                else
                {
                    return _accountManager
                        .AcquireUserLogon(new UserLogon
                        {
                            User = new User { UserId = context.Identity.Name },
                            Client = new Account.Models.UserAgent
                            {
                                OS = agent.OS.Family,
                                OSVersion = $"{agent.OS.Major}.{agent.OS.Minor}",

                                Browser = agent.UserAgent.Family,
                                BrowserVersion = $"{agent.UserAgent.Major}.{agent.UserAgent.Minor}",

                                Device = $"{agent.Device.Family}"
                            },
                            SecurityToken = _token,
                            Location = null,
                            IPAddress = null, //eventually find a way to get at the ipaddress

                            ModifiedOn = DateTime.Now
                        })
                        .Resolve();
                }
            });
        });

        /// <summary>
        /// For custom authentication/authorizations
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task GrantCustomExtension(OAuthGrantCustomExtensionContext context) => base.GrantCustomExtension(context);

        //public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) => Task.Run(() => context.Validated());


        /// <summary>
        /// This method should be overriden if the user want's to apply extra verification on the user object.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual IOperation<ClaimsIdentity> ExtendedUserVerification(ClaimsIdentity identity, User user, IServiceResolver resolver) => LazyOp.Try(() => identity);

        public void Dispose()
        {
        }
        #endregion
    }
}
