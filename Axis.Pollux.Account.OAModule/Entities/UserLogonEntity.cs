using Axis.Pollux.Common;
using Axis.Pollux.Identity.OAModule.Entities;

namespace Axis.Pollux.Account.OAModule.Entities
{
    public class UserLogonEntity: PolluxEntity<long>
    {
        public UserAgentEntity Client { get; set; } = new UserAgentEntity();
        public string Location { get; set; }
        public string SecurityToken { get; set; }
        public bool Invalidated { get; set; }
        public string IPAddress { get; set; }

        /// <summary>
        /// </summary>
        public int TimeZoneOffset { get; set; }

        //[MaxLength(20)]
        public string Locale { get; set; }

        public virtual UserEntity User { get; set; }
        public string UserId { get; set; }
    }

    //[ComplexType]
    public class UserAgentEntity
    {
        public string OS { get; set; }
        public string OSVersion { get; set; }

        public string Browser { get; set; }
        public string BrowserVersion { get; set; }

        public string Device { get; set; }
    }
}
