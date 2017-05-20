using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.Services
{
    public interface IUserAttributeManager
    {
        Operation<IAttribute> AssignAttribute(string userId, SubjectAuthorizationAttribute attribute);
        Operation<IAttribute> RemoveAttribute(string userId, string attributeName);
    }
}
