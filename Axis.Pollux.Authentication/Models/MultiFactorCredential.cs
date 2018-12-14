using System;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Authentication.Models
{
    public class MultiFactorCredential: BaseModel<Guid>
    {
        public User TargetUser { get; set; }
        public string CredentialKey { get; set; }
        public string CredentialToken { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// A string representing the entity - usually a contract operation call, that the multi-factor authentication is being employed to guard"
        /// </summary>
        public string EventLabel { get; set; }
    }
}
