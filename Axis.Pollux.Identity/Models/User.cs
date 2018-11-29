using System;
using System.Collections.Generic;
using System.Text;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Identity.Models
{
    public class User: BaseModel<Guid>
    {
        public int Status { get; set; }

        public BioData Bio { get; set; }
        public ContactData[] ContactInfo { get; set; }
        public NameInfo[] Names { get; set; }
        public UserData[] Data { get; set; }
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
}
