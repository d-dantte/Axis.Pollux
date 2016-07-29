using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.ABAC.OAModule.Mappings
{
    public class PolicySetMapping: BaseMap<PolicySet, int>
    {
        public PolicySetMapping()
        {
            Property(p => p.CombinationClause).HasMaxLength(250);
            Property(p => p.Name).HasMaxLength(250).IsIndex($"{nameof(PolicySet)}_{nameof(PolicySet.Name)}");
            Property(p => p.TargetExpression).IsMaxLength();
            Property(p => p.Title).HasMaxLength(250);

            //relationship
            this.HasMany(e => e.Policies);
            this.HasMany(e => e.SubSets);
        }
    }
}
