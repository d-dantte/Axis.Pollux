using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class CorporateDataMap: BaseMap<CorporateData, long>
    {
        public CorporateDataMap()
        {
            ///Configure Properties
            this.Property(e => e.CorporateId).HasMaxLength(250);

            ///Conifgure Relationships.

            //one way to owner
            this.HasRequired(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }
    }
}
