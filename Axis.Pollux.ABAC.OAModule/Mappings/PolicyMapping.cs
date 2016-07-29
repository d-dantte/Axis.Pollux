using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.ABAC.OAModule.Mappings
{
    public class PolicyMapping: BaseMap<Policy,int>
    {
        public PolicyMapping()
        {
            Property(p => p.CombinationClause).HasMaxLength(250);
            Property(p => p.Name).HasMaxLength(250).IsIndex($"{nameof(Policy)}_{nameof(Policy.Name)}");
            Property(p => p.TargetExpression).IsMaxLength();
            Property(p => p.Title).HasMaxLength(250);

            //relationship
            this.HasMany(e => e.Rules);
        }
    }
}
