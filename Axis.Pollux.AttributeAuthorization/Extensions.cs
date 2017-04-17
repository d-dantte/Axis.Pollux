using Axis.Luna;
using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.AttributeAuthorization
{
    public static class Extensions
    {
        public static SubjectAuthorizationAttribute ToSubjectAttribute(this IDataItem userData)
        => new SubjectAuthorizationAttribute
        {
            Data = userData.Data,
            Name = userData.Name,
            Type = userData.Type
        };

        public static UserData ToUserData(this SubjectAuthorizationAttribute attribute, User owner)
        => new UserData
        {
            Data = attribute.Data,
            Name = attribute.Name,
            Owner = owner,
            Type = attribute.Type
        };
    }
}
