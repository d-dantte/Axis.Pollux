using Axis.Luna;
using Axis.Luna.Extensions;
using Axis.Pollux.Common;
using Axis.Pollux.Identity;
using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;
using System.Linq;

namespace Axis.Pollux.Audit.Models
{
    public class AuditEntry: PolluxModel<long>, IUserIdentified
    {
        public User User { get; set; }

        public string Action { get; set; }

        /// <summary>
        /// Represents anciliary information about the user and his/her capabilities at the time he/she performed the action been logged
        /// This is a css-formated name/value pair list.
        /// </summary>
        public string ContextData { get; set; }


        public IEnumerable<KeyValuePair<string, string>> ContextValues()
        => TagBuilder
            .Create(ContextData ?? "").Tags
            .Select(_t => _t.Name.ValuePair(_t.Value));
    }
}
