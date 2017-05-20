using Axis.Pollux.ABAC.Services;
using System.Collections.Generic;
using System.Linq;
using Axis.Luna;
using Axis.Sigma.Core;
using Axis.Pollux.Common.Services;
using Axis.Pollux.ABAC.Auth;

using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.ABAC.RolePermissionPolicy.Services.Impl
{
    public class UserAttributeSource : IUserAttributeSource
    {
        private IUserContext _userContext;

        public UserAttributeSource(IUserContext userContext)
        {
            ThrowNullArguments(() => userContext);
            
            _userContext = userContext;
        }


        public Operation<IEnumerable<IAttribute>> GetAttributes()
        => Operation.Try(() =>
        {
            return _userContext
                .UserAttributes()
                .Select(_ditem => new SubjectAuthorizationAttribute(_ditem))
                .Cast<IAttribute>()
                .ThrowIf(RequiredAttributesAreMissing, "Some required attributes are missing from this attribute source");
        });


        private bool RequiredAttributesAreMissing(IEnumerable<IAttribute> attributes)
        => !attributes.Any(_att => _att.Name == CommonAttributeNames.SubjectAttribute_UserName ||
                                   _att.Name == CommonAttributeNames.SubjectAttribute_UserRole);
    }
}
