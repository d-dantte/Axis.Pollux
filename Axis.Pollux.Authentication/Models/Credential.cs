using Axis.Pollux.Identity.Principal;
using System;
using Axis.Luna.Extensions;
using static Axis.Luna.Extensions.ObjectExtensions;
using static Axis.Luna.Extensions.ExceptionExtensions;
using Axis.Luna.Operation;
using Axis.Luna.Utils;

namespace Axis.Pollux.Authentication.Models
{
    public class Credential: PolluxModel<long>
    {
        public virtual CredentialMetadata Metadata { get; set; } = CredentialMetadata.Password;
        
        public byte[] Value { get; set; }

        public string SecuredHash { get; set; } //hash of data if required is kept here

        public DateTime? ExpiresOn { get; set; }

        /// <summary>
        /// inline css formatted name/value pairs
        /// </summary>
        public string Tags { get; set; }


        public virtual User Owner { get; set; }

        public Credential()
        { }

        public override IOperation Validate() => LazyOp.Try(() =>
        {

        });
    }

    public class CredentialMetadata: IValidatable, IEquatable<CredentialMetadata>
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

        public CredentialMetadata(string name, Access access = Access.Secret)
        {
            ThrowNullArguments(() => name);

            this.Name = name;
            this.Access = access;
        }

        public CredentialMetadata()
        { }

        #region properties
        public string Name { get; set; }
        public Access Access { get; set; }
        #endregion

        public override bool Equals(object obj) => Equals(obj.Cast<CredentialMetadata>());
        public bool Equals(CredentialMetadata other)
            => other.Name == null ? false :
               this.Name == other.Name &&
               this.Access == other.Access;

        public override int GetHashCode() => ValueHash(17, 23, Name, Access);

        public override string ToString() => $"{{Name:'{Name}', Access:'{Access}'}}";

        public IOperation Validate()
        => LazyOp.Try(() =>
        {
            string.IsNullOrWhiteSpace(Name)
                  .ThrowIf(_inws => _inws, "Invalid Metadata Name");
        });
    }
    
    public enum Access
    {
        Public,
        Secret
    }
}
