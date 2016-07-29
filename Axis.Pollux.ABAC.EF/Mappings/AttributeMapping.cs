using Axis.Pollux.Maps;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.ABAC.EF.Mappings
{
    public class AttributeMapping: BaseMap<Auth.Attribute, int>
    {
        public AttributeMapping(): base(false)
        {
            Map(m => m.ToTable("PolicyAttribute"));

            Property(e => e.Name).HasMaxLength(250);
            Property(e => e.Value).IsMaxLength();
        }
    }
}
