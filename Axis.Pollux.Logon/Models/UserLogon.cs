using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Exceptions;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Identity.Models;

namespace Axis.Pollux.Logon.Models
{
    public class UserLogon : BaseModel<Guid>, IUserOwned
    {
        public UserAgent Client { get; set; } = new UserAgent();

        public string IpAddress { get; set; }

        public string SessionToken { get; set; }
        public bool Invalidated { get; set; }

        /// <summary>
        /// Represents an offset in minutes that needs to be subtracted from the LOCAL time to get UTC time.
        /// UTC +1h will have a value of +60. NewYork will have a value of -300.
        /// </summary>
        public int TimeZoneOffset { get; set; }

        public string Locale { get; set; }

        public virtual User User { get; set; }

        public virtual User Owner => User;


        public override Operation Validate()
        => base.Validate().Then(() =>
        {
            if (User == null)
                throw new CommonException(ErrorCodes.ModelValidationError);

            if (string.IsNullOrWhiteSpace(SessionToken))
                throw new CommonException(ErrorCodes.ModelValidationError);
        });
    }

    public class UserAgent
    {
        public string OS { get; set; }
        public string OSVersion { get; set; }

        public string Browser { get; set; }
        public string BrowserVersion { get; set; }

        public string Device { get; set; }
    }
}
