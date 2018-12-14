using System;
using Axis.Luna.Operation;
using Axis.Pollux.Authentication.Contracts.Params;
using Axis.Pollux.Authentication.Models;

namespace Axis.Pollux.Authentication.Contracts
{
    public interface ICredentialManager
    {
        /// <summary>
        /// Check that the credential info given matches those of any stored in the back end using the following rules
        /// 1. Search the back end for credentials belonging the info.UserId having name = info.Name.
        /// 2. Looping through all the "active" credentials found in the 1 above, compare the "Data" properties.
        /// 3. For credentials with "Secret" visibility, hash the info.Data before comparing with the credential.
        /// 4. Return safely for a match found. Throw an error for every other scenario.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        Operation ValidateNonUniqueCredential(NonUniqueCredentialValidationInfo info);


        /// <summary>
        /// Check that the credential info given matches those of any stored in the back end using the following rules
        /// 1. Search the back end for UNIQUE credentials having name = info.Name.
        /// 2. Looping through all the "active" credentials found in the 1 above, compare the "Data" properties.
        /// 3. For credentials with "Secret" visibility, hash the info.Data before comparing with the credential.
        /// 4. Return safely for a match found. Throw an error for every other scenario.
        /// </summary>
        /// <param name="info"></param>
        /// <returns>The id of the user to whom the credential belongs</returns>
        Operation<Guid> ValidateUniqueCredential(UniqueCredentialValidationInfo info);

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
