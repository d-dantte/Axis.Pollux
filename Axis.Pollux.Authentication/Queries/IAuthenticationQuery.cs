

namespace Axis.Pollux.Authentication.Queries
{
    public interface IAuthenticationQuery
    {
        bool UserExists(string userId);
        Credential GetCredential(string userId, CredentialMetadata metadata);
    }
}
