using System;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Contracts.Params;
using Axis.Pollux.Authentication.Models;

namespace Axis.Pollux.Authentication.Contracts
{
    public interface ICredentialManager
    {
        /// <summary>
        /// Adds a new credential for the specified user. If the Visibility is set to "Secret", the credential is
        /// hashed and the hash is used. If the "IsUnique" flag is set, the system is searched if another credential
        /// exists with the same name and data values as the ones supplied; if found, an exception is thrown
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="credential"></param>
        /// <returns></returns>
        Operation AddCredential(Guid userId, Credential credential);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialId"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        Operation UpdateCredentialStatus(Guid credentialId, int newStatus);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialId"></param>
        /// <returns></returns>
        Operation<bool> IsCredentialExpired(Guid credentialId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credentialId"></param>
        /// <returns></returns>
        Operation<int> CredentialStatus(Guid credentialId);
    }
}
