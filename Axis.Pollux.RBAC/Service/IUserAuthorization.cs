using System.Linq;
using Axis.Luna;
using Axis.Pollux.RBAC.Auth;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.RBAC.Services
{
    public interface IUserAuthorization
    {
        Operation AddRole(string role);

        Operation AssignRole(User user, string role);

        IQueryable<Role> UserRoles(User user);

        Operation Authorize(PermissionProfile authRequest);

        Operation<Role> CreateRole(string name);

        Operation DeleteUserRole(User user, Role role);

        Operation DeleteRole(Role role);
    }
}
