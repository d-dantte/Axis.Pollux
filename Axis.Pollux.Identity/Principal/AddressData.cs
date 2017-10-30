using Axis.Pollux.Common;

namespace Axis.Pollux.Identity.Principal
{
    public class AddressData : PolluxModel<long>, IUserOwned
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }

        public AddressStatus Status { get; set; }

        #region navigational properties
        public User Owner { get; set; }
        #endregion
    }

    public enum AddressStatus
    {
        Active,
        Archived
    }
}
