using Axis.Pollux.ABAC.Services;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Sigma.Core;
using Axis.Jupiter.Commands;
using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna.Extensions;
using Axis.Pollux.ABAC.RolePermissionPolicy.Queries;
using Axis.Pollux.ABAC.RolePermissionPolicy;

namespace Axis.Pollux.ABAC.DAS.Services
{
    public class PrincipalRoleManager : IUserAttributeManager
    {
        private IPersistenceCommands _pcommands;
        private IPrincipalRoleQuery _query;

        public PrincipalRoleManager(IPersistenceCommands pcommands, IPrincipalRoleQuery query)
        {
            ThrowNullArguments(() => pcommands,
                               () => query);

            _pcommands = pcommands;
            _query = query;
        }

        public IOperation<IAttribute> AssignAttribute(string userId, IAttribute attribute)
        => LazyOp.Try(() =>
        {
            var user = _query.GetUser(userId).ThrowIfNull("invalid user id");
            return attribute
                .Cast<UserRole>()
                .ThrowIfNull("Invalid Attribute")
                .UsingValue(_ur => _ur.User = user)
                .Pipe(_ur => _pcommands.Add(_ur).Resolve())
                .Cast<IAttribute>();
        });

        public IOperation<IAttribute> RemoveAttribute(string userId, string attributeName)
        => LazyOp.Try(() =>
        {
            return _query
                .GetUserRole(userId, attributeName)
                .ThrowIfNull("Invalid Attribute")
                .Pipe(_ur => _pcommands.Delete(_ur).Resolve())
                .Cast<IAttribute>();
        });
    }
}
