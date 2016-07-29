using System;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.Auth
{
    public abstract class AuthorizationAttribute : PolluxEntity<int>
    {
        public string Name
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public string Owner
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public Category Category
        {
            get { return get<Category>(); }
            set { set(ref value); }
        }
    }

    public abstract class AuthorizationAttribute<V>: AuthorizationAttribute
    {
        public abstract V Value { get; set; }
    }

    #region Concrete Attributes
    public class IntegralAttribute : AuthorizationAttribute<long>
    {
        public override long Value
        {
            get { return get<long>(); }
            set { set(ref value); }
        }
    }
    public class RealAttribute : AuthorizationAttribute<double>
    {
        public override double Value
        {
            get { return get<double>(); }
            set { set(ref value); }
        }
    }
    public class DecimalAttribute : AuthorizationAttribute<decimal>
    {
        public override decimal Value
        {
            get { return get<long>(); }
            set { set(ref value); }
        }
    }
    public class StringAttribute : AuthorizationAttribute<string>
    {
        public override string Value
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
    }
    public class BinaryAttribute : AuthorizationAttribute<byte[]>
    {
        public override byte[] Value
        {
            get { return get<byte[]>(); }
            set { set(ref value); }
        }
    }
    public class BooleanAttribute : AuthorizationAttribute<bool>
    {
        public override bool Value
        {
            get { return get<bool>(); }
            set { set(ref value); }
        }
    }
    public class DateTimeAttribute : AuthorizationAttribute<DateTime?>
    {
        public override DateTime? Value
        {
            get { return get<DateTime?>(); }
            set { set(ref value); }
        }
    }
    public class TimeSpanAttribute : AuthorizationAttribute<TimeSpan?>
    {
        public override TimeSpan? Value
        {
            get { return get<TimeSpan?>(); }
            set { set(ref value); }
        }
    }
    public class GuidAttribute : AuthorizationAttribute<Guid>
    {
        public override Guid Value
        {
            get { return get<Guid>(); }
            set { set(ref value); }
        }
    }
    #endregion
}
