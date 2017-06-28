using Axis.Luna.Operation;

namespace Axis.Pollux.Authentication.Service
{
    public interface ICredentialAuthentication
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

        IOperation ModifyCredential(Credential oldValue, Credential newValue);
    }
}
