using Axis.Pollux.ABAC.Services;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core;
using System.Collections.Generic;
using Axis.Luna.Operation;

namespace Axis.Pollux.ABAC.Auth
{
    public class AuthorizablePrincipal: User, IAttributeSource
    {
        private List<IAttribute> _attributes = new List<IAttribute>();
        public IEnumerable<IAttribute> SubjectAttributes
        {
            get { return _attributes; }
            set
            {
                _attributes.Clear();
                if (value != null) _attributes.AddRange(value);
            }
        }

        public IOperation<IEnumerable<IAttribute>> GetAttributes() => LazyOp.Try(() => SubjectAttributes);
    }
}
