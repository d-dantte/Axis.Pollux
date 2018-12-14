using System;
using System.Threading.Tasks;
using Axis.Pollux.Authentication.Models;
using Axis.Pollux.Common.Utils;

namespace Axis.Pollux.Authentication.Services.Queries
{
    public interface IAuthenticatorQueries
    {
        Task<ArrayPage<Credential>> GetActiveUserCredentials(Guid ownerId, string credentialName, ArrayPageRequest request = null);
        Task<ArrayPage<Credential>> GetActiveSystemUniqueCredentials(string credentialName, ArrayPageRequest request = null);
        Task<bool> ContainsPublicCredentials(Guid userId, string credentialName, string credentialData);
        Task<bool> ContainsPublicCredentials(string credentialName, string credentialData);
        Task<Credential> GetCredentialById(Guid credentialId);
    }
}
