using Axis.Pollux.ABAC.DAS.Models;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.DAS.Queries
{
    public interface IPrincipalRoleQuery
    {
        IEnumerable<UserRole> GetUserRoles(string userId);
        UserRole GetUserRole(string userId, string roleName);
    }
}
