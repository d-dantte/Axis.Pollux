using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.Principal
{
    public class ContactData: PolluxEntity<long>
    {
        public string Phone  { get { return get<string>(); } set { set(ref value); } }
        public string AlternatePhone  { get { return get<string>(); } set { set(ref value); } }
        public bool PhoneConfirmed { get { return get<bool>(); } set { set(ref value); } }

        public string Email  { get { return get<string>(); } set { set(ref value); } }
        public string AlternateEmail  { get { return get<string>(); } set { set(ref value); } }
        public bool EmailConfirmed  { get { return get<bool>(); } set { set(ref value); } }

        public ContactStatus Status { get; set; }

        #region navigational properties
        public virtual User Owner  { get { return get<User>(); } set { set(ref value); } }
        public virtual string OwnerId  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        public ContactData()
        {
        }
    }

    public enum ContactStatus
    {
        Current,
        Archived
    }
}
