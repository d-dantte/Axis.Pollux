using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.Principal
{
    public class AddressData : PolluxEntity<long>
    {
        public virtual string Street
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public virtual string City
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public virtual string StateProvince
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public virtual string Country
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        #region navigational properties
        public virtual User Owner
        {
            get { return get<User>(); }
            set { set(ref value); }
        }
        public virtual string OwnerId
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        #endregion

        public AddressData()
        {
        }
    }
}
