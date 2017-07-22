using Axis.Pollux.Common;
using Axis.Pollux.Identity;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Account.Models
{
    public class UserLogon: PolluxModel<long>, IUserIdentified
    {
        public UserAgent Client { get; set; } = new UserAgent();
        public string Location { get; set; }
        public string SecurityToken { get; set; }
        public bool Invalidated { get; set; }
        
        /// <summary>
        /// Represents an offset in minutes that needs to be subtracted from the LOCAL time to get UTC time.
        /// UTC +1h will have a value of +60. NewYork will have a value of -300.
        /// </summary>
        public int TimeZoneOffset { get; set; }

        //[MaxLength(20)]
        public string Locale { get; set; }
        
        public virtual User User { get; set; }
    }

    //[ComplexType]
    public class UserAgent
    {
        public string OS { get; set; }
        public string OSVersion { get; set; }

        public string Browser { get; set; }
        public string BrowserVersion { get; set; }

        public string Device { get; set; }
    }
}
