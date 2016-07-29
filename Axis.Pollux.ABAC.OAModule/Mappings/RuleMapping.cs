using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.ABAC.OAModule.Mappings
{
    public class RuleMapping : BaseMap<Rule, int>
    {
        public RuleMapping()
        {
            Property(p => p.Name).HasMaxLength(250).IsIndex($"{nameof(Rule)}_{nameof(Rule.Name)}");
            Property(p => p.Title).HasMaxLength(250);
            Property(p => p.ActionCondition).IsMaxLength();
            Property(p => p.ResourceCondition).IsMaxLength();
            Property(p => p.EnvironmentCondition).IsMaxLength();
            Property(p => p.SubjectCondition).IsMaxLength();
        }
    }
}
