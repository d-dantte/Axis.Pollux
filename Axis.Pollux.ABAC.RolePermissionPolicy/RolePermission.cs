using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.RolePermissionPolicy
{
    public class RolePermission: PolluxModel<long>
    {
        public Sigma.Core.Policy.Effect Effect { get; set; }
        public string IntentDescriptor { get; set; }
        public string RoleName { get; set; }
        public string PolicyCode { get; set; }
    }
}
