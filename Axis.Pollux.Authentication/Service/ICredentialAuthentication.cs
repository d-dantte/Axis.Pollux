using Axis.Luna;
using Axis.Pollux.Identity.Principal;
using System.Linq;

namespace Axis.Pollux.Authentication.Service
{
    public interface ICredentialAuthentication
    {
        ICredentialHasher CredentialHasher { get; }
        
        Operation VerifyCredential(Credential credential);
        
        Operation DeleteCredential(Credential credential);

        Operation AssignCredential(string userId, Credential credential);

        Operation ModifyCredential(Credential oldValue, byte[] newValue);
    }
}
