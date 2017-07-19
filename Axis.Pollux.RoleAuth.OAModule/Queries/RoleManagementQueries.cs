using Axis.Pollux.RoleManagement.Queries;
using System.Collections.Generic;
using Axis.Pollux.RoleAuth.Models;
using Axis.Jupiter.Europa;

using static Axis.Luna.Extensions.ExceptionExtensions;
using System.Linq;
using Axis.Pollux.RoleAuth.OAModule.Entities;
using System;

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
            .Query<RoleEntity>()
            .Transform<RoleEntity, Role>(_europa);

        public RolePermission GetPermissionForUUID(Guid uuid)
        => _europa
            .Query<RolePermissionEntity>(_rpe => _rpe.Role)
            .FirstOrDefault(_rpe => _rpe.UUID == uuid)
            .Transform<RolePermissionEntity, RolePermission>(_europa);

        public IEnumerable<RolePermission> GetPermissionsForLabel(string label)
        => _europa
            .Query<RolePermissionEntity>(_rpe => _rpe.Role)
            .Where(_rp => _rp.Label == label)
            .Transform<RolePermissionEntity, RolePermission>(_europa);

        public IEnumerable<RolePermission> GetPermissionsForResource(string resource)
        => _europa
            .Query<RolePermissionEntity>(_rpe => _rpe.Role)
            .Where(_rp => _rp.Resource == resource)
            .Transform<RolePermissionEntity, RolePermission>(_europa);

        public IEnumerable<RolePermission> GetPermissionsForRole(string roleName)
        => _europa
            .Query<RolePermissionEntity>(_rpe => _rpe.Role)
            .Where(_rp => _rp.RoleName == roleName)
            .Transform<RolePermissionEntity, RolePermission>(_europa);

        public UserRole GetUserRole(string userId, string roleName)
        => _europa
            .Query<UserRoleEntity>(_ur => _ur.Role, _ur => _ur.User)
            .Where(_ur => _ur.RoleName == roleName)
            .Where(_ur => _ur.UserId == userId)
            .FirstOrDefault()
            .Transform<UserRoleEntity, UserRole>(_europa);


        public IEnumerable<UserRole> GetUserRolesFor(string userId)
        => _europa
            .Query<UserRoleEntity>(_ur => _ur.Role, _ur => _ur.User)
            .Where(_ur => _ur.UserId == userId)
            .Transform<UserRoleEntity, UserRole>(_europa);
    }
}
