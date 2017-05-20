

using Axis.Pollux.Authentication;

namespace Axis.Pollux.CoreAuthentication.Queries
{
    public interface IAuthenticationQuery
    {
        bool UserExists(string userId);
        Credential GetCredential(string userId, CredentialMetadata metadata);
    }
}
