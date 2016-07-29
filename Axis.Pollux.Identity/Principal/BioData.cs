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
        public virtual string FirstName  { get { return get<string>(); } set { set(ref value); } }
        public virtual string MiddleName  { get { return get<string>(); } set { set(ref value); } }
        public virtual string LastName  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        #region Bio
        public virtual DateTime? Dob  { get { return get<DateTime?>(); } set { set(ref value); } }
        public virtual Gender Gender  { get { return get<Gender>(); } set { set(ref value); } }

        public virtual string Nationality  { get { return get<string>(); } set { set(ref value); } }
        public virtual string StateOfOrigin  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        #region navigational properties
        public virtual User Owner  { get { return get<User>(); } set { set(ref value); } }
        public virtual string OwnerId  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        public BioData()
        {
        }
    }

    public enum Gender { Female, Male, Other}
}
