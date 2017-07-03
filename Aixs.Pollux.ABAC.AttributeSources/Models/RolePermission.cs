using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.DAS.Models
{
    public class RolePermission: PolluxModel<long>
    {
        public Sigma.Core.Policy.Effect Effect { get; set; }
        public string IntentDescriptor { get; set; }
        public string RoleName { get; set; }

        /// <summary>
        /// Simple string identifier for identifying/grouping sets of permissions
        /// </summary>
        public string Label { get; set; }
    }
}
