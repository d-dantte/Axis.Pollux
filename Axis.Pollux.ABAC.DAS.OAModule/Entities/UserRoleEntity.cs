using Axis.Pollux.Identity.OAModule.Entities;

namespace Axis.Pollux.ABAC.DAS.OAModule.Entities
{
    public class UserRoleEntity: PolluxEntity<long>
    {
        public UserEntity User { get; set; }
        public string UserId { get; set; }

        public string RoleName { get; set; }
    }
}
