using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.Account.Objects
{
    public class ContextVerification: PolluxEntity<long>
    {
        private User _target;
        private string _targetId;
        public virtual User Target
        {
            get { return _target; }
            set
            {
                _target = value;
                if (value != null) _targetId = _target.EntityId;
                else _targetId = null;
            }
        }
        public string TargetId
        {
            get { return _targetId; }
            set
            {
                _targetId = value;
                if (value == null) _target = null;
                else if (!value.Equals(_target?.EntityId)) _target = null;
            }
        }

        public virtual string VerificationToken { get; set; }

        public virtual bool Verified { get; set; }

        public virtual string Context { get; set; }

        public DateTime ExpiresOn { get; set; }
    }
}
