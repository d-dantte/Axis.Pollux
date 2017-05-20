using System.Linq;
using Axis.Luna;
using Axis.Pollux.RBAC.Auth;
using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;

namespace Axis.Pollux.RBAC.Services
{
    using PermissionMap = Dictionary<string, IEnumerable<Permission>>;


    public interface IUserAuthorization
    {
        Operation AddRole(string role);

        Operation AssignRole(User user, string role);

        IQueryable<Role> UserRoles(User user);

        PermissionMap UserPermissions(User user);

        Operation AuthorizeAccess(PermissionProfile authRequest, Func<Operation> operation = null);

        Operation<T> AuthorizeAccess<T>(PermissionProfile authRequest, Func<Operation<T>> operation = null);

        Operation<T> AuthorizeAccess<T>(PermissionProfile authRequest, Func<T> operation = null);

        Operation AuthorizeAccess(PermissionProfile authRequest, Action operation = null);

        Operation<Role> CreateRole(string name);

        Operation DeleteUserRole(User user, Role role);

        Operation DeleteRole(Role role);
    }
}
