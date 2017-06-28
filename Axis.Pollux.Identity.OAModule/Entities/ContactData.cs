using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.OAModule.Entities
{
    public class ContactDataEntity : PolluxEntity<long>
    {
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public bool PhoneConfirmed { get; set; }

        public string Email { get; set; }
        public string AlternateEmail { get; set; }
        public bool EmailConfirmed { get; set; }

        public ContactStatus Status { get; set; }

        #region navigational properties
        public virtual UserEntity Owner { get; set; }
        public string OwnerId { get; set; }
        #endregion
    }
}
