using Axis.Luna.Operation;
using Axis.Pollux.AccessAuthority.Models;

namespace Axis.Pollux.AccessAuthority
{
    public interface IDataAccessAuthority
    {
        IOperation AuthorizeAccess<PrincipalId>(ResourceDescriptor descriptor);
    }
}
