using Axis.Luna.Operation;
using Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.Services
{
    public interface IUserAttributeManager
    {
        IOperation<IAttribute> AssignAttribute(string userId, IAttribute attribute);
        IOperation<IAttribute> RemoveAttribute(string userId, string attributeName);
    }
}
