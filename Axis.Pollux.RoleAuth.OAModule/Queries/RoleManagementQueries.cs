using Axis.Pollux.RoleManagement.Queries;
using Axis.Pollux.RoleAuth.Models;
using Axis.Jupiter.Europa;

using static Axis.Luna.Extensions.ExceptionExtensions;
using System.Linq;
using Axis.Pollux.RoleAuth.OAModule.Entities;
using System;
using Axis.Pollux.UserCommon.Models;
using Axis.Luna;

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

        public SequencePage<Role> GetAllRoles(PageParams pageParams = null)
        {
            var q = _europa
                .Query<RoleEntity>()
                .OrderBy(_ad => _ad.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(q, _q => _q.Transform<RoleEntity, Role>(_europa));
        }

        public RolePermission GetPermissionForUUID(Guid uuid)
        => _europa
            .Query<RolePermissionEntity>(_rpe => _rpe.Role)
            .FirstOrDefault(_rpe => _rpe.UUID == uuid)
            .Transform<RolePermissionEntity, RolePermission>(_europa);

        public SequencePage<RolePermission> GetPermissionsForLabel(string label, PageParams pageParams = null)
        {
            if (string.IsNullOrWhiteSpace(label)) throw new Exception("invalid label");
            var q = _europa
                .Query<RolePermissionEntity>(_rp => _rp.Role)
                .Where(_rp => _rp.Label == label)
                .OrderBy(_ad => _ad.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(q, _q => _q.Transform<RolePermissionEntity, RolePermission>(_europa));
        }

        public SequencePage<RolePermission> GetPermissionsForResource(string resource, PageParams pageParams = null)
        {
            if (string.IsNullOrWhiteSpace(resource)) throw new Exception("invalid resource");
            var q = _europa
                .Query<RolePermissionEntity>(_rp => _rp.Role)
                .Where(_rp => _rp.Resource == resource)
                .OrderBy(_ad => _ad.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(q, _q => _q.Transform<RolePermissionEntity, RolePermission>(_europa));
        }

        public SequencePage<RolePermission> GetPermissionsForRole(string roleName, PageParams pageParams = null)
        {
            if (string.IsNullOrWhiteSpace(roleName)) throw new Exception("invalid role name");
            var q = _europa
                .Query<RolePermissionEntity>(_rp => _rp.Role)
                .Where(_rp => _rp.RoleName == roleName)
                .OrderBy(_ad => _ad.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(q, _q => _q.Transform<RolePermissionEntity, RolePermission>(_europa));
        }

        public UserRole GetUserRole(string userId, string roleName)
        => _europa
            .Query<UserRoleEntity>(_ur => _ur.Role, _ur => _ur.User)
            .Where(_ur => _ur.RoleName == roleName)
            .Where(_ur => _ur.UserId == userId)
            .FirstOrDefault()
            .Transform<UserRoleEntity, UserRole>(_europa);


        public SequencePage<UserRole> GetUserRolesFor(string userId, PageParams pageParams = null)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new Exception("invalid use id");
            var q = _europa
                .Query<UserRoleEntity>(_ur => _ur.Role, _ur => _ur.User)
                .Where(_rp => _rp.UserId == userId)
                .OrderBy(_ad => _ad.CreatedOn);

            pageParams = pageParams ?? PageParams.EntireSequence();

            return pageParams.Paginate(q, _q => _q.Transform<UserRoleEntity, UserRole>(_europa));
        }
    }
}
