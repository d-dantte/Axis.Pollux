using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Account.Objects
{
    public class UserLogon: PolluxEntity<long>
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

        private User _user;
        private string _userId;
        public virtual User User
        {
            get { return _user; }
            set
            {
                _user = value;
                if (value != null) _userId = _user.EntityId;
                else _userId = null;
            }
        }
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                if (value == null) _user = null;
                else if (!value.Equals(_user?.EntityId)) _user = null;
            }
        }
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
