using System;
using System.Collections.Generic;
using System.Text;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Identity.Models
{
    public class BioData: BaseModel<Guid>, IUserOwned
    {
        public DateTimeOffset? DateOfBirth { get; set; }
        public string Gender { get; set; }

        public string CountryOfBirth { get; set; }

        public User Owner { get; set; }
    }
}
