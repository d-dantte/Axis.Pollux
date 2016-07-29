using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class ContactDataMap: BaseMap<ContactData, long>
    {
        public ContactDataMap()
        {
            ///Configure Properties
            this.Property(e => e.AlternateEmail).HasMaxLength(250);
            this.Property(e => e.AlternatePhone).HasMaxLength(50);
            this.Property(e => e.Email).HasMaxLength(250);
            this.Property(e => e.Phone).HasMaxLength(50);


            ///Conifgure Relationships.            
            this.HasRequired(e => e.Owner)//one way to owner
                .WithMany()
                .HasForeignKey(e => e.OwnerId);
        }
    }
}
