using Axis.Luna;

namespace Axis.Pollux.Authentication.Service
{
    public interface ICredentialAuthentication
    {
        ICredentialHasher CredentialHasher { get; }
        
        Operation VerifyCredential(Credential credential);
        
        Operation DeleteCredential(Credential credential);

        Operation AssignCredential(string userId, Credential credential);

        Operation ModifyCredential(Credential modifiedCredential);
    }
}
