using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using System.Linq;

namespace Axis.Pollux.ABAC.Services
{
    public interface IAttributeStore
    {
        IQueryable<AuthorizationAttribute> SubjectAttributes();
        IQueryable<AuthorizationAttribute> EnvironmentAttributes();
        IQueryable<AuthorizationAttribute> ActionAttributes();
        IQueryable<AuthorizationAttribute> ResourceAttributes();

        Operation<IAttributeStore> Add(AuthorizationAttribute attribute);
        Operation Delete(AuthorizationAttribute attribute, bool commit = false);
        IQueryable<AuthorizationAttribute> Modify(AuthorizationAttribute dobj);
    }
}
