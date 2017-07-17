using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using Axis.Luna.Operation;
using Axis.Sigma.Core;
using Axis.Pollux.Common.Services;
using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.ABAC.RolePermissionPolicy.Queries;

namespace Axis.Pollux.ABAC.DAS.Services
{
    public class PrincipalRoleSource : IUserAttributeSource
    {
        private IPrincipalRoleQuery _query;
        private IUserContext _userContext;

        public PrincipalRoleSource(IUserContext context, IPrincipalRoleQuery query)
        {
            ThrowNullArguments(() => context, () => query);

            _query = query;
            _userContext = context;
        }


        public IOperation<IEnumerable<IAttribute>> GetAttributes()
        => LazyOp.Try(() => _query.GetUserRoles(_userContext.User().UserId));
    }
}
