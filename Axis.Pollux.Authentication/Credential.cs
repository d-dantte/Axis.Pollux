using Axis.Pollux.Identity.Principal;
using System;
using Axis.Luna.Extensions;
using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.ExceptionExtensions;

namespace Axis.Pollux.Authentication
{
    public class Credential: PolluxEntity<long>
    {
        public virtual CredentialMetadata Metadata
        {
            get { return get<CredentialMetadata>(); }
            set { set(ref value); }
        }
        
        public virtual byte[] Value
        {
            get { return get<byte[]>(); }
            set { set(ref value); }
        }

        public virtual string SecuredHash
        {
            get { return get<string>(); }
            set { set(ref value); }
        } //hash of data if required is kept here

        public virtual TimeSpan? ExpiresIn
        {
            get { return get<TimeSpan?>(); }
            set { set(ref value); }
        }

        public CredentialStatus Status
        {
            get { return get<CredentialStatus>(); }
            set { set(ref value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Tags
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        #region navigational properties
        public virtual User Owner  { get { return get<User>(); } set { set(ref value); } }
        public virtual string OwnerId  { get { return get<string>(); } set { set(ref value); } }
        #endregion

        public Credential()
        { }
    }

    public class CredentialMetadata: IEquatable<CredentialMetadata>
    {
        #region standard credential metadata
        public static readonly CredentialMetadata Password = new CredentialMetadata() { Name = nameof(Password), Access = Access.Secret };
        
        public static readonly CredentialMetadata LeftEye = new CredentialMetadata() { Name = nameof(LeftEye), Access = Access.Secret };
        public static readonly CredentialMetadata RightEye = new CredentialMetadata() { Name = nameof(RightEye), Access = Access.Secret };
        
        public static readonly CredentialMetadata LeftThumb = new CredentialMetadata() { Name = nameof(LeftThumb), Access = Access.Secret };
        public static readonly CredentialMetadata LeftIndex = new CredentialMetadata() { Name = nameof(LeftIndex), Access = Access.Secret };
        public static readonly CredentialMetadata LeftMiddle = new CredentialMetadata() { Name = nameof(LeftMiddle), Access = Access.Secret };
        public static readonly CredentialMetadata LeftRing = new CredentialMetadata() { Name = nameof(LeftRing), Access = Access.Secret };
        public static readonly CredentialMetadata LeftLittle = new CredentialMetadata() { Name = nameof(LeftLittle), Access = Access.Secret };
        
        public static readonly CredentialMetadata RightThumb = new CredentialMetadata() { Name = nameof(RightThumb), Access = Access.Secret };
        public static readonly CredentialMetadata RightIndex = new CredentialMetadata() { Name = nameof(RightIndex), Access = Access.Secret };
        public static readonly CredentialMetadata RightMiddle = new CredentialMetadata() { Name = nameof(RightMiddle), Access = Access.Secret };
        public static readonly CredentialMetadata RightRing = new CredentialMetadata() { Name = nameof(RightRing), Access = Access.Secret };
        public static readonly CredentialMetadata RightLittle = new CredentialMetadata() { Name = nameof(RightLittle), Access = Access.Secret };
        
        public static readonly CredentialMetadata Other = new CredentialMetadata() { Name = nameof(Other), Access = Access.Public };
        #endregion

        private CredentialMetadata()
        { }

        public CredentialMetadata(string name, Access access = Access.Secret)
        {
            ThrowNullArguments(() => name);

            this.Name = name;
            this.Access = access;
        }

        #region properties
        public string Name { get; private set; }
        public Access Access { get; private set; }
        #endregion

        public override bool Equals(object obj) => Equals(obj.As<CredentialMetadata>());
        public bool Equals(CredentialMetadata other)
            => other.Name == null ? false :
               this.Name == other.Name &&
               this.Access == other.Access;

        public override int GetHashCode() => ValueHash(17, 23, Name, Access);

        public override string ToString() => $"{{Name:'{Name}', Access:'{Access}'}}";
    }
    
    public enum Access
    {
        Public,
        Secret
    }

    public enum CredentialStatus
    {
        Active,
        Inactive
    }
}
