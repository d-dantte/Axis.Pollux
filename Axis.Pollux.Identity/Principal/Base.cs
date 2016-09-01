using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.ExceptionExtensions;

using System;
using System.ComponentModel;
using Axis.Narvi.Notify;

namespace Axis.Pollux.Identity.Principal
{
    public abstract class PolluxEntity<Key>: NotifierBase, IEquatable<PolluxEntity<Key>>
    {
        public virtual Key EntityId
        {
            get { return get<Key>(); }
            set { set(ref value); }
        }

        public virtual DateTime CreatedOn
        {
            get { return get<DateTime>(); }
            set { set(ref value); }
        }

        public virtual DateTime? ModifiedOn
        {
            get { return get<DateTime?>(); }
            set { set(ref value); }
        }

        protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(sender, e);
            if (e.PropertyName != nameof(ModifiedOn) &&
                e.PropertyName != nameof(CreatedOn)) this.ModifiedOn = DateTime.Now;
        }

        public virtual bool Equals(PolluxEntity<Key> other) => other?.EntityId.Equals(EntityId) ?? false;

        public override bool Equals(object obj) => Equals(obj.As<PolluxEntity<Key>>());
        public override int GetHashCode() => Eval(() => EntityId.GetHashCode());


        public PolluxEntity()
        {
            this.CreatedOn = DateTime.Now;
        }
    }
}
