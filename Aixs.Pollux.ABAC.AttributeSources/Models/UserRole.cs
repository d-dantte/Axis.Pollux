using Axis.Luna.Utils;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core;
using Axis.Luna.Extensions;

namespace Axis.Pollux.ABAC.DAS.Models
{
    public class UserRole: PolluxModel<long>, IAttribute
    {
        public User User { get; set; }

        public string RoleName { get; set; }
        

        #region IAttribute Members
        public AttributeCategory Category => AttributeCategory.Subject;

        public string Name
        {
            get { return typeof(UserRole).FullName; }

            set { }
        }

        public string Data
        {
            get { return RoleName; }

            set { RoleName = value; }
        }

        public CommonDataType Type
        {
            get { return CommonDataType.String; }

            set { }
        }
        
        public object Clone() => Copy();

        public IAttribute Copy() => new UserRole
        {
            User = User,
            Data = Data,
            CreatedOn = CreatedOn,
            ModifiedOn = ModifiedOn,
            UniqueId = UniqueId
        };

        public V ResolveData<V>() => Data.Cast<V>(); //<-- any "V" other than "string" should throw an exception
        #endregion
    }
}
