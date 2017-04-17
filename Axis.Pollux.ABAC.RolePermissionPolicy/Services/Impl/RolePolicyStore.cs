using Axis.Pollux.ABAC.RolePermissionPolicy.Sesrvices;
using System;
using System.Collections.Generic;
using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core.Policy;
using Axis.Pollux.ABAC.RolePermissionPolicy.Queries;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl
{
    public class RolePolicyStore : IRolePolicyStore
    {
        private IRolePermissionQuery _query;

        public RolePolicyStore(IRolePermissionQuery query)
        {
            ThrowNullArguments(() => query);

            _query = query;
        }


        #region RolePolicyStore

        public Operation<RolePermission> AddPolicyRule(RolePermission permission)
        {
            throw new NotImplementedException();
        }

        public Operation<IEnumerable<RolePermission>> GetPolicyRule(string policy)
        {
            throw new NotImplementedException();
        }

        public Operation<IEnumerable<RolePermission>> GetUserPolicyRule(User user, string policy)
        {
            throw new NotImplementedException();
        }

        public Operation<RolePermission> RemovePolicyRule(RolePermission permission)
        {
            throw new NotImplementedException();
        }

        public Operation<RolePermission> UpdatePolicyRule(RolePermission permission)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region PolicyIO

        public Operation Persist(IEnumerable<Policy> policies)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Policy> Policies()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
