
using Axis.Luna.Extensions;
using Axis.Luna.Operation;
using Axis.Luna.Utils;
using System;

namespace Axis.Pollux.Identity.Principal
{
    public abstract class PolluxModel<Key> : IEquatable<PolluxModel<Key>>, IValidatable
    {
        public Key UniqueId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public virtual IOperation Validate() => ResolvedOp.Try(() => { });

        public virtual bool Equals(PolluxModel<Key> other) => other?.UniqueId.Equals(UniqueId) ?? false;

        public override bool Equals(object obj) => Equals(obj.Cast<PolluxModel<Key>>());
        public override int GetHashCode() => ResolvedOp.Try(() => UniqueId.GetHashCode()).Result;


        public PolluxModel()
        {
            CreatedOn = DateTime.Now;
        }
    }
}
