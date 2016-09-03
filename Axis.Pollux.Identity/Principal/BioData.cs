using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.Principal
{
    public class BioData: PolluxEntity<long>
    {
        #region Names
        public string FirstName  { get { return get<string>(); } set { set(ref value); } }
        public string MiddleName  { get { return get<string>(); } set { set(ref value); } }
        public string LastName  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        #region Bio
        public DateTime? Dob  { get { return get<DateTime?>(); } set { set(ref value); } }
        public Gender Gender  { get { return get<Gender>(); } set { set(ref value); } }

        public string Nationality  { get { return get<string>(); } set { set(ref value); } }
        public string StateOfOrigin  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        #region navigational properties
        public virtual User Owner  { get { return get<User>(); } set { set(ref value); } }
        public string OwnerId  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        public BioData()
        {
        }
    }

    public enum Gender { Female, Male, Other}
}
