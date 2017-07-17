using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.RoleAuth.Models;
using System;

namespace Axis.Pollux.RoleAuth.OAModule.Entities
{
    public class RolePermissionEntity: PolluxEntity<long>
    {
        public Role Role { get; set; }
        public string RoleName { get; set; }

        public PermissionEffect Effect { get; set; }
        public string Resource { get; set; }

        public string Label { get; set; }
        public Guid UUID { get; set; }

        public RolePermissionEntity()
        {
            UUID = Guid.NewGuid();
            Effect = PermissionEffect.Deny;
        }
    }
}
