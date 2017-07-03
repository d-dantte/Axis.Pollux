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
        public string RoleName { get; set; }
        
        public string Label { get; set; }
    }
}
