using Axis.Pollux.ABAC.Services;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Sigma.Core;
using Axis.Jupiter.Commands;
using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna.Extensions;
using Axis.Pollux.ABAC.DAS.Models;
using Axis.Pollux.ABAC.DAS.Queries;

namespace Axis.Pollux.ABAC.DAS.Services
{
    public class PrincipalRoleManager : IUserAttributeManager
    {
        private IPersistenceCommands _pcommands;
        private IPrincipalRoleQuery _query;

        public PrincipalRoleManager(IPersistenceCommands pcommands)
        {
            ThrowNullArguments(() => pcommands);

            _pcommands = pcommands;
        }

        public IOperation<IAttribute> AssignAttribute(string userId, IAttribute attribute)
        => LazyOp.Try(() =>
        {
            return attribute
                .Cast<UserRole>()
                .ThrowIfNull("Invalid Attribute")
                .UsingValue(_ur => _ur.User = new Identity.Principal.User { UserId = userId })
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
