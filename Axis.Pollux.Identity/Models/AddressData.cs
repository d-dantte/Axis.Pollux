using System;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Identity.Models
{
    public class AddressData: BaseModel<Guid>, IUserOwned
    {
        public string PostCode { get; set; }
        public string Flat { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }

        public int Status { get; set; }

        public User Owner { get; set; }
    }

    public enum AddressStatus
    {
        Archived = 0,
        Active = 1
    }
}
