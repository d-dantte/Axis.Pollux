using Axis.Pollux.Identity.Principal;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.Auth
{
    public class PolicySet: PolluxEntity<int>
    {
        public string Name
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public string Title
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public string TargetExpression
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public string CombinationClause
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public virtual ICollection<Policy> Policies { get; set; }
        public virtual ICollection<PolicySet> SubSets { get; set; }
    }
}
