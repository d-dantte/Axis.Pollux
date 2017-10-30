using Axis.Pollux.Common;

namespace Axis.Pollux.Identity.Principal
{
    public class ContactData : PolluxModel<long>, IUserOwned
    {
        public User Owner { get; set; }

        public ContactChannel Channel { get; set; }
        public string Value { get; set; }
        public ContactStatus Status { get; set; }
        public bool IsPrimary { get; set; }
    }

    public enum ContactChannel
    {
        Mobile,
        Fax,
        Email,
        POBox,
        PMBox,

        /// <summary>
        /// A Special contact-channel representing the system.
        /// Notifications sent to this channel will show up as SystemNotifications
        /// </summary>
        System
    }

    public enum ContactStatus
    {
        Unverified,
        Active,
        Archived
    }
}
