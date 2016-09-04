using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.Principal
{
    public class CorporateData: PolluxEntity<long>
    {
        #region Identity
        public string CorporateName  { get { return get<string>(); } set { set(ref value); } }
        public string CorporateId  { get { return get<string>(); } set { set(ref value); } } //stuff like cac number, etc. unique
        #endregion

        #region Misc
        public string Description
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        #endregion

        #region Misc
        public virtual DateTime? IncorporationDate  { get { return get<DateTime?>(); } set { set(ref value); } }
        #endregion

        #region navigational properties
        public virtual User Owner  { get { return get<User>(); } set { set(ref value); } }
        public virtual string OwnerId  { get { return get<string>(); } set { set(ref value); } }
        #endregion
    }
}
