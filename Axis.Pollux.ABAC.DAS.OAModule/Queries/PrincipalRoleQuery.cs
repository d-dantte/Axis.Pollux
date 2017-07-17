using Axis.Jupiter.Europa;
using Axis.Pollux.ABAC.DAS.Queries;
using System.Collections.Generic;
using System.Linq;
using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Pollux.ABAC.DAS.Models;
using Axis.Pollux.ABAC.DAS.OAModule.Entities;
using Axis.Luna.Extensions;

namespace Axis.Pollux.ABAC.DAS.OAModule.Queries
{
    public class PrincipalRoleQuery: IPrincipalRoleQuery
    {
        private DataStore _europa;

        public PrincipalRoleQuery(DataStore store)
        {
            ThrowNullArguments(() => store);

            this._europa = store;
        }

        public UserRole GetUserRole(string userId, string roleName)
        => _europa.Query<UserRoleEntity>(_ur => _ur.User)
                  .Where(_ur => _ur.UserId == userId)
                  .Where(_ur => _ur.RoleName == roleName)
                  .FirstOrDefault()?
                  .Pipe(new ModelConverter(_europa).ToModel<UserRole>);


        public IEnumerable<UserRole> GetUserRoles(string userId)
        => new ModelConverter(_europa)
            .Pipe(_c => _europa
            .Query<UserRoleEntity>(_ur => _ur.User)
            .Where(_ur => _ur.UserId == userId)
            .AsEnumerable()
            .Select(_c.ToModel<UserRole>));
    }
}
