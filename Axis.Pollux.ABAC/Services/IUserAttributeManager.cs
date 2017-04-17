using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core;
using System.Collections.Generic;

namespace Axis.Pollux.ABAC.Services
{
    public interface IUserAttributeManager
    {
        Operation<IAttribute> AssignAttribute(User user, SubjectAuthorizationAttribute attribute);
        Operation<IAttribute> AddAttribute(SubjectAuthorizationAttribute attribute);

        Operation<IAttribute> UpdateAttribute(SubjectAuthorizationAttribute attribute);
        Operation<IAttribute> RemoveAttribute(string attributeName);

        Operation<IEnumerable<IAttribute>> GetAttributes();
        Operation<IEnumerable<IAttribute>> GetAttributesFor(User user);
    }
}
