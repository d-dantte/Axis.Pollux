using Axis.Pollux.Common;
using Axis.Pollux.RoleAuth.Models;

namespace Axis.Pollux.RoleAuth.OAModule.Entities
{
    public class RoleEntity: PolluxEntity<string>
    {
        public RoleStatus Status { get; set; }
    }
}
