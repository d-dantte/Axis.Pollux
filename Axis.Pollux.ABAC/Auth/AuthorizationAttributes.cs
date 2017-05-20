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

        public override string ToString()
        => $"[Name: {Name}, Type: {Type}, Data: {Data}]";

        public override int GetHashCode() => this.PropertyHash();
    }


    public class SubjectAuthorizationAttribute : AuthorizationAttribute
    {
        public override sealed AttributeCategory Category => AttributeCategory.Subject;
        public override IAttribute Copy(AttributeCategory category) => new SubjectAuthorizationAttribute().CopyFrom(this);
        public override bool Equals(object obj)
        {
            var other = obj as SubjectAuthorizationAttribute;
            return other != null &&
                   other.Data?.Equals(Data) == true &&
                   other.Name?.Equals(Name) == true &&
                   other.Type.Equals(Type);
        }

        #region init
        public SubjectAuthorizationAttribute()
        { }
        public SubjectAuthorizationAttribute(IDataItem dataItem)
        {
            Name = dataItem.Name;
            Type = dataItem.Type;
            Data = dataItem.Data;
        }
        #endregion
    }


    public class IntentAuthorizationAttribute : AuthorizationAttribute
    {
        public override sealed AttributeCategory Category => AttributeCategory.Intent;
        public override IAttribute Copy(AttributeCategory category) => new IntentAuthorizationAttribute().CopyFrom(this);
        public override bool Equals(object obj)
        {
            var other = obj as IntentAuthorizationAttribute;
            return other != null &&
                   other.Data?.Equals(Data) == true &&
                   other.Name?.Equals(Name) == true &&
                   other.Type.Equals(Type);
        }

        #region init
        public IntentAuthorizationAttribute()
        { }
        public IntentAuthorizationAttribute(IDataItem dataItem)
        {
            Name = dataItem.Name;
            Type = dataItem.Type;
            Data = dataItem.Data;
        }
        #endregion
    }


    public class AccessContextAuthorizationAttribute : AuthorizationAttribute
    {
        public override sealed AttributeCategory Category => AttributeCategory.Environment;
        public override IAttribute Copy(AttributeCategory category) => new AccessContextAuthorizationAttribute().CopyFrom(this);
        public override bool Equals(object obj)
        {
            var other = obj as AccessContextAuthorizationAttribute;
            return other != null &&
                   other.Data?.Equals(Data) == true &&
                   other.Name?.Equals(Name) == true &&
                   other.Type.Equals(Type);
        }

        #region init
        public AccessContextAuthorizationAttribute()
        { }
        public AccessContextAuthorizationAttribute(IDataItem dataItem)
        {
            Name = dataItem.Name;
            Type = dataItem.Type;
            Data = dataItem.Data;
        }
        #endregion
    }
}
