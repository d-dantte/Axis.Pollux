using Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl.Queries;
using Axis.Sigma.Core.Policy;
using Axis.Jupiter;

using static Axis.Luna.Extensions.ExceptionExtensions;
using System.Linq;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.EF.Queries
{
    public class RolePermissionQuery : IRolePermissionQuery
    {
        private IDataContext _dataStore;

        public RolePermissionQuery(IDataContext dataStore)
        {
            ThrowNullArguments(() => dataStore);

            _dataStore = dataStore;
        }

        public IEnumerable<RolePermission> GetAllPolicyRules()
        => _dataStore.Store<RolePermission>().Query
                     .ToArray();

        public IEnumerable<RolePermission> GetPolicyRules(string policyCode)
        => _dataStore.Store<RolePermission>().Query
                     .Where(_rp => _rp.PolicyCode == policyCode)
                     .ToArray();

        public RolePermission GetRolePermission(string policyCode, string roleName, string intentDescriptor, Effect effect)
        => _dataStore.Store<RolePermission>().Query
                     .Where(_rp => _rp.PolicyCode == policyCode)
                     .Where(_rp => _rp.RoleName == roleName)
                     .Where(_rp => _rp.IntentDescriptor == intentDescriptor)
                     .Where(_rp => _rp.Effect == effect)
                     .FirstOrDefault();

        public RolePermission GetRolePermissionById(long id)
        => _dataStore.Store<RolePermission>().Query
                     .Where(_rp => _rp.UniqueId == id)
                     .FirstOrDefault();

        public IEnumerable<RolePermission> GetUserPolicyRules(string userId, string policyCode)
        => (from rp in _dataStore.Store<RolePermission>().Query
            join ur in _dataStore.Store<UserRole>().Query on rp.RoleName equals ur.RoleName
            where rp.PolicyCode == policyCode && ur.UserId == userId
            select rp)
           .ToArray();
    }
}
