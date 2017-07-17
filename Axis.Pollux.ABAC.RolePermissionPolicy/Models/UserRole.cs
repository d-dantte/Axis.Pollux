using Axis.Luna.Utils;
using Axis.Pollux.Identity.Principal;
using Axis.Sigma.Core;
using Axis.Luna.Extensions;

namespace Axis.Pollux.ABAC.RolePermissionPolicy
{
    public class UserRole: PolluxModel<long>, IAttribute
    {
        public User User { get; set; }

        public string RoleName { get; set; }

        #region IAttribute Members
        public AttributeCategory Category => AttributeCategory.Subject;

        public string Name { get; set; }

        public string Data
        {
            get { return RoleName; }

            set { RoleName = value; }
        }

        public CommonDataType Type { get; set; }

        public V ResolveData<V>() => Data.Cast<V>();

        public object Clone() => Copy();

        public IAttribute Copy()
        {
            return new UserRole
            {
                CreatedOn = CreatedOn,
                Data = Data,
                ModifiedOn = ModifiedOn,
                Name = Name,
                RoleName = RoleName,
                UniqueId = UniqueId,
                User = User
            };
        }
        #endregion

        #region Init
        public UserRole()
        {
            Name = typeof(UserRole).FullName;
            Type = CommonDataType.String;
        }
        #endregion
    }
}
