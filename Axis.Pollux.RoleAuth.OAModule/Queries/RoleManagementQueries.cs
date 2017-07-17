using Axis.Pollux.RoleManagement.Queries;
using System;
using System.Collections.Generic;
using Axis.Pollux.RoleAuth.Models;
using Axis.Jupiter.Europa;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.RoleAuth.OAModule.Queries
{
    public class RoleManagementQueries : IRoleManagementQueries
    {
        private DataStore _europa;

        public RoleManagementQueries(DataStore store)
        {
            ThrowNullArguments(() => store);

            _europa = store;
        }

        public IEnumerable<Role> GetAllRoles()
        => _europa

        public IEnumerable<RolePermission> GetPermissionsForLabel(string label)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RolePermission> GetPermissionsForResource(string resource)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RolePermission> GetPermissionsForRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public UserRole GetUserRole(string userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserRole> GetUserRolesFor(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
