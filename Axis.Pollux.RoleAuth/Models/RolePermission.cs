using Axis.Pollux.Common;
using System;

namespace Axis.Pollux.RoleAuth.Models
{
    public class RolePermission: PolluxModel<long>
    {
        public Role Role { get; set; }

        public PermissionEffect Effect { get; set; }

        public string Resource { get; set; }
        public string Intent { get; set; }
        public string Context { get; set; }

        public string Label { get; set; }
        public Guid UUID { get; set; }

        public RolePermission()
        {
            UUID = Guid.NewGuid();
            Effect = PermissionEffect.Deny;
        }
    }

    public enum PermissionEffect
    {
        Deny,
        Grant
    }
}
