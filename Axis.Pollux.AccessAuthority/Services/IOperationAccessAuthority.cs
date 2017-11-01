using Axis.Luna.Operation;
using Axis.Pollux.AccessAuthority.Models;

namespace Axis.Pollux.AccessAuthority
{
    public interface IOperationAccessAuthority
    {
        IOperation AuthorizeAccess<PrincipalId>(ResourceDescriptor descriptor);
    }
}
