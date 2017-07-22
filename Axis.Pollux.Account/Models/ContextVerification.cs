using Axis.Pollux.Common;
using Axis.Pollux.Identity;
using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.Account.Models
{
    public class ContextVerification: PolluxModel<long>, IUserTargeted
    {
        public virtual User Target { get; set; }

        public string VerificationToken { get; set; }

        public bool Verified { get; set; }

        public string Context { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}
