using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Models;

namespace Axis.Pollux.Authentication.Services
{
    public interface ICredentialAuthority
    {
        ICredentialHasher CredentialHasher { get; }
        
        IOperation VerifyCredential(Credential credential);
        
        IOperation ExpireCredential(Credential credential);

        /// <summary>
        /// Assigns the supplied credential to the user 
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        IOperation AssignCredential(Credential credential);
    }
}
