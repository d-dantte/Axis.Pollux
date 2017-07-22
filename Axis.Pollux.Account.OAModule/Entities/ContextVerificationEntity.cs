using Axis.Pollux.Common;
using Axis.Pollux.Identity.OAModule.Entities;
using System;

namespace Axis.Pollux.Account.OAModule.Entities
{
    public class ContextVerificationEntity: PolluxEntity<long>
    {
        public UserEntity Target { get; set; }
        public string TargetId { get; set; }

        public string VerificationToken { get; set; }

        public bool Verified { get; set; }

        public string Context { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}
