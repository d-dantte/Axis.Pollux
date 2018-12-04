using System;
using Axis.Pollux.Common.Models;
using Axis.Pollux.Common.Utils;

namespace Axis.Pollux.Identity.Models
{
    public class User: BaseModel<Guid>
    {
        public int Status { get; set; }
    }

    /// <summary>
    /// Possible status values for the user
    /// </summary>
    public enum UserStatus
    {
        Inactive = 0,
        Active = 1,
        Blocked = 2
    }


    /// <summary>
    /// This represents a user profile. It is not a "BaseModel" descendant, and as such, not expected to exist in the
    /// Data-Store
    /// </summary>
    public class UserProfile
    {
        public User User { get; set; }

        public BioData Bio { get; set; }
        public ArrayPage<AddressData> Addresses { get; set; }
        public ArrayPage<ContactData> ContactInfo { get; set; }
        public ArrayPage<NameData> Names { get; set; }
        public ArrayPage<UserData> Data { get; set; }
    }
}
