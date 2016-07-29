using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class BioDataMap: BaseMap<BioData, long>
    {
        public BioDataMap()
        {
            ///Configure Properties
            this.Property(e => e.FirstName).HasMaxLength(100);
            this.Property(e => e.LastName).HasMaxLength(100);
            this.Property(e => e.MiddleName).HasMaxLength(100);
            this.Property(e => e.Nationality).HasMaxLength(250);
            this.Property(e => e.StateOfOrigin).HasMaxLength(250);

            ///Conifgure Relationships.
            this.HasRequired(e => e.Owner) //one way to owner
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }
    }
}
