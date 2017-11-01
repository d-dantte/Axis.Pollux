using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Luna.Operation;
using Axis.Pollux.AccessAuthority;
using Axis.Pollux.AccessAuthority.Models;
using Axis.Pollux.Identity.Services;
using Axis.Pollux.RoleAuth.Models;
using Axis.Pollux.RoleAuth.Services;
using Axis.Pollux.Utils.Services;
using Axis.Proteus;
using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.RBAC.Services
{
    public class DataAccessAuthority : IDataAccessAuthority
    {
        private IUserContext _userContext;
        private ICache _cache;
        private IServiceResolver _resolver;


        public DataAccessAuthority(IUserContext userContext, IServiceResolver resolver)
        {
            ThrowNullArguments(() => userContext, () => resolver);

            _userContext = userContext;
            _resolver = resolver;
        }

        public IOperation AuthorizeAccess<PrincipalId>(ResourceDescriptor descriptor)
        => LazyOp.Try(() =>
        {
            var _user = _userContext.User();
            return _cache.GetOrAdd(_userContext.User().UserName, _descriptor =>
            {
                var roleManager = _resolver.Resolve<IRoleManager>();
                var roles = roleManager.GetUserRolesFor(_user.UniqueId).Resolve().Page;
                return roles
                    .Select(_r => roleManager.GetPermissionsFor(_r.UniqueId))
                    .Select(_op => _op.Resolve().ThrowIfNull("Invalid Query"))
                    .SelectMany(_page => _page.Page)
                    .ToArray();
            })
            .Then(_permissions =>
            {
                //aggregate and evaluate the access right
                _permissions
                    .Where(_p => _p.Resource == "*" || _p.Resource == descriptor.Resource)
                    .Where(_p => _p.Intent == "*" || _p.Intent == descriptor.Intent)
                    .Where(_p => _p.Context == "*" || _p.Context.Contains(descriptor.Context))
                    .Select(_p => _p.Effect)
                    .Combine()
                    .ThrowIf(PermissionEffect.Deny, "Access Denied");
            });
        });
    }
}
