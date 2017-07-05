using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Sigma.Core.Policy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.ABAC.DAS.OAModule.Entities
{
    public class RolePermissionEntity: PolluxEntity<long>
    {
        public Effect Effect { get; set; }
        public string IntentDescriptor { get; set; }

        /// <summary>
        /// Note that there is no ForeignKey relationship between this entity and the User Role entity.
        /// Both are "loosely" linked by this RoleName property and as such, it will be made an Index.
        /// </summary>
        public string RoleName { get; set; }        
        public string Label { get; set; }
    }
}
