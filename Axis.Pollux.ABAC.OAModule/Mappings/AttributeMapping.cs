using Axis.Pollux.Identity.OAModule.Mappings;
using Axis.Jupiter.Europa;

namespace Axis.Pollux.ABAC.OAModule.Mappings
{
    public abstract class AttributeMapping: BaseMap<Auth.AuthorizationAttribute, int>
    {
        public AttributeMapping()
        {
            Property(e => e.Name).HasMaxLength(250).IsIndex($"{nameof(Auth.AuthorizationAttribute)}_{nameof(Auth.AuthorizationAttribute.Name)}");

            Map<Auth.IntegralAttribute>(m => m.Property(p => p.Value).HasColumnName("IntegralValue"));
            Map<Auth.RealAttribute>(m => m.Property(p => p.Value).HasColumnName("RealValue"));
            Map<Auth.DecimalAttribute>(m => m.Property(p => p.Value).HasColumnName("DecimalValue"));
            Map<Auth.StringAttribute>(m => m.Property(p => p.Value).HasColumnName("StringValue"));
            Map<Auth.BinaryAttribute>(m => m.Property(p => p.Value).HasColumnName("BinaryValue"));
            Map<Auth.BooleanAttribute>(m => m.Property(p => p.Value).HasColumnName("BooleanValue"));
            Map<Auth.DateTimeAttribute>(m => m.Property(p => p.Value).HasColumnName("DateTimeValue"));
            Map<Auth.TimeSpanAttribute>(m => m.Property(p => p.Value).HasColumnName("TimeSpanValue"));
            Map<Auth.GuidAttribute>(m => m.Property(p => p.Value).HasColumnName("GuidValue"));
        }
    }
}
