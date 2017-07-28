using Axis.Pollux.Common;
using Axis.Pollux.Identity.OAModule.Entities;

namespace Axis.Pollux.Audit.OAModule.Entities
{
    public class AuditEntryEntity: PolluxEntity<long>
    {
        public UserEntity User { get; set; }
        public string UserId { get; set; }

        public string Action { get; set; }
        public string ContextData { get; set; }
    }
}
