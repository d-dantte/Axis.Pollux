using System;

namespace Axis.Pollux.Identity.Principal
{
    public class User: PolluxEntity<string>
    {

        public User()
        {
            UId = Guid.NewGuid();
        }
        
        public virtual string UserId
        {
            get { return this.EntityId; }
            set { this.EntityId = value; }
        }

        public virtual int Status
        {
            get { return get<int>(); }
            set { set(ref value); }
        }

        public virtual Guid UId
        {
            get { return get<Guid>(); }
            set { set(ref value); }
        }
    }
}
