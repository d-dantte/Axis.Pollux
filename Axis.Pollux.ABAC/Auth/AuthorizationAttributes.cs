using Axis.Sigma.Core;
using Axis.Luna.Extensions;
using Axis.Luna.Utils;

namespace Axis.Pollux.ABAC.Auth
{
    /// <summary>
    /// Default (abstract) implementation of the Authorization Attribute
    /// </summary>
    public abstract class AuthorizationAttribute : IAttribute
    {
        public CommonDataType Type { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }

        public R ResolveData<R>() => this.ParseData<R>();
        public object Clone() => Copy();

        public abstract IAttribute Copy();
        public AttributeCategory Category { get; private set; }

        #region init
        protected AuthorizationAttribute(AttributeCategory category)
        {
            this.Category = category;
        }
        #endregion

        public override string ToString()
        => $"[Name: {Name}, Type: {Type}, Data: {Data}]";

        public override int GetHashCode() => this.PropertyHash();
    }


    /// <summary>
    /// Default implementation of the Subject Authorization Attribute
    /// </summary>
    public class SubjectAuthorizationAttribute : AuthorizationAttribute
    {
        public override IAttribute Copy() => new SubjectAuthorizationAttribute().CopyFrom(this);
        public override bool Equals(object obj)
        {
            var other = obj as SubjectAuthorizationAttribute;
            return other != null &&
                   other.Data?.Equals(Data) == true &&
                   other.Name?.Equals(Name) == true &&
                   other.Type.Equals(Type);
        }
        public override int GetHashCode() => base.GetHashCode();

        #region init
        public SubjectAuthorizationAttribute()
        :base(AttributeCategory.Subject)
        { }
        public SubjectAuthorizationAttribute(IDataItem dataItem) //<-- shouldn't this be IAttribute, instead of IDataItem?
        : base(AttributeCategory.Subject)
        {
            Name = dataItem.Name;
            Type = dataItem.Type;
            Data = dataItem.Data;
        }
        #endregion
    }


    /// <summary>
    /// Default implementation of the Intent Authorization Attribute
    /// </summary>
    public class IntentAuthorizationAttribute : AuthorizationAttribute
    {
        public override IAttribute Copy() => new IntentAuthorizationAttribute().CopyFrom(this);
        public override bool Equals(object obj)
        {
            var other = obj as IntentAuthorizationAttribute;
            return other != null &&
                   other.Data?.Equals(Data) == true &&
                   other.Name?.Equals(Name) == true &&
                   other.Type.Equals(Type);
        }
        public override int GetHashCode() => base.GetHashCode();

        #region init
        public IntentAuthorizationAttribute()
        : base(AttributeCategory.Intent)
        { }
        public IntentAuthorizationAttribute(IDataItem dataItem)
        : base(AttributeCategory.Intent)
        {
            Name = dataItem.Name;
            Type = dataItem.Type;
            Data = dataItem.Data;
        }
        #endregion
    }
    

    /// <summary>
    /// Deafult implementation of the Environment Authorization Attribute
    /// </summary>
    public class EnvironmentAuthorizationAttribute : AuthorizationAttribute
    {
        public override IAttribute Copy() => new EnvironmentAuthorizationAttribute().CopyFrom(this);
        public override bool Equals(object obj)
        {
            var other = obj as EnvironmentAuthorizationAttribute;
            return other != null &&
                   other.Data?.Equals(Data) == true &&
                   other.Name?.Equals(Name) == true &&
                   other.Type.Equals(Type);
        }
        public override int GetHashCode() => base.GetHashCode();

        #region init
        public EnvironmentAuthorizationAttribute()
        : base(AttributeCategory.Environment)
        { }
        public EnvironmentAuthorizationAttribute(IDataItem dataItem)
        : base(AttributeCategory.Environment)
        {
            Name = dataItem.Name;
            Type = dataItem.Type;
            Data = dataItem.Data;
        }
        #endregion
    }
}
