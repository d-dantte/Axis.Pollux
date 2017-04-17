using Axis.Sigma.Core;
using Axis.Luna;
using Axis.Luna.Extensions;

namespace Axis.Pollux.ABAC.Auth
{
    public abstract class AuthorizationAttribute : IAttribute
    {
        public CommonDataType Type { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }

        public R ResolveData<R>() => this.ParseData<R>();
        public object Clone() => Copy(Category);

        public abstract IAttribute Copy(AttributeCategory category);
        public abstract AttributeCategory Category { get; }

        #region init
        protected AuthorizationAttribute()
        { }
        #endregion
    }


    public class SubjectAuthorizationAttribute : AuthorizationAttribute
    {
        public override AttributeCategory Category => AttributeCategory.Subject;
        public override IAttribute Copy(AttributeCategory category) => new SubjectAuthorizationAttribute().CopyFrom(this);

        #region init
        public SubjectAuthorizationAttribute()
        { }
        #endregion
    }


    public class IntentAuthorizationAttribute : AuthorizationAttribute
    {
        public override AttributeCategory Category => AttributeCategory.Intent;
        public override IAttribute Copy(AttributeCategory category) => new IntentAuthorizationAttribute().CopyFrom(this);

        #region init
        public IntentAuthorizationAttribute()
        { }
        #endregion
    }


    public class AccessContextAuthorizationAttribute : AuthorizationAttribute
    {
        public override AttributeCategory Category => AttributeCategory.Environment;
        public override IAttribute Copy(AttributeCategory category) => new AccessContextAuthorizationAttribute().CopyFrom(this);

        #region init
        public AccessContextAuthorizationAttribute()
        { }
        #endregion
    }
}
