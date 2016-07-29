using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Pollux.Identity.Principal
{
    public class UserData: PolluxEntity<long>
    {
        public virtual string StringData  { get { return get<string>(); } set { set(ref value); } }
        public virtual byte[] BinaryData  { get { return get<byte[]>(); } set { set(ref value); } }

        public virtual string Name { get { return get<string>(); } set { set(ref value); } }

        #region Navigation Properties
        public virtual User Owner  { get { return get<User>(); } set { set(ref value); } }
        public virtual string OwnerId  { get { return get<string>(); } set { set(ref value); } }
        #endregion


        public UserData()
        {
        }
    }
}
