using Axis.Jupiter.Europa;
using Axis.Pollux.Identity.Principal;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public class UserDataMap: BaseMap<UserData, long>
    {
        public UserDataMap()
        {
            ///Conifgure Relationships.

            //one way to owner
            this.HasRequired(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId);

            this.Property(e => e.Name).IsIndex("UserDataName");
        }
    }
}
