using Axis.Jupiter;
using Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl.Queries;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.EF.Queries
{
    public class UserRoleQuery : IUserRoleQuery
    {
        private IDataContext _data;

        public UserRoleQuery(IDataContext dataContext)
        {
            ThrowNullArguments(() => dataContext);

            _data = dataContext;
        }

        public UserRole GetUserRole(string userId, string roleName)
        => _data.Store<UserRole>()
                .QueryWith(_ur => _ur.User)
                .Where(_ur => _ur.RoleName == roleName)
                .Where(_ur => _ur.UserId == userId)
                .FirstOrDefault();
    }
}
