using Axis.Pollux.Authentication.Models;
using System.Collections.Generic;

namespace Axis.Pollux.CoreAuthentication.Queries
{
    public interface IAuthenticationQuery
    {
        bool UserExists(string userId);
        IEnumerable<Credential> GetCredentials(string userId, CredentialMetadata metadata);
    }
}
