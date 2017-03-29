using Axis.Jupiter;
using Axis.Pollux.Authentication.Queries;
using Axis.Pollux.Identity.Principal;
using System.Linq;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.Authentication.OAModule.Queries
{
    public class AuthenticationQuery: IAuthenticationQuery
    {
        private IDataContext _europa = null;

        public AuthenticationQuery(IDataContext context)
        {
            ThrowNullArguments(() => context);

            _europa = context;
        }

        public bool UserExists(string userId)
        => _europa.Store<User>().Query.Any(_u => _u.EntityId == userId);

        public Credential GetCredential(string userId, CredentialMetadata metadata)
        => _europa.Store<Credential>().Query
                  .Where(_cred => _cred.Metadata.Name == metadata.Name)
                  .Where(_cred => _cred.Metadata.Access == metadata.Access)
                  .Where(_cred => _cred.OwnerId == userId)
                  .OrderByDescending(_cred => _cred.CreatedOn)
                  .FirstOrDefault();
    }
}
