using Axis.Jupiter.Europa;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Authentication.OAModule.Entities;
using Axis.Pollux.CoreAuthentication.Queries;
using Axis.Pollux.Identity.OAModule.Entities;
using System.Collections.Generic;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.Authentication.OAModule.Queries
{
    public class AuthenticationQuery: IAuthenticationQuery
    {
        private DataStore _europa = null;

        public AuthenticationQuery(DataStore context)
        {
            ThrowNullArguments(() => context);

            _europa = context;
        }

        public bool UserExists(string userId)
        => _europa.Query<UserEntity>().Any(_u => _u.UniqueId == userId);

        public IEnumerable<Credential> GetCredentials(string userId, CredentialMetadata metadata)
        => _europa.Query<CredentialEntity>(_cred => _cred.Owner)
                  .Where(_cred => _cred.Metadata.Name == metadata.Name)
                  .Where(_cred => _cred.Metadata.Access == metadata.Access)
                  .Where(_cred => _cred.OwnerId == userId)
                  .OrderByDescending(_cred => _cred.CreatedOn)
                  .AsEnumerable()
                  .Select(new ModelConverter(_europa).ToModel<Credential>);
    }
}
