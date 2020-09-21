using Axis.Pollux.Common.Models;
using System;

namespace Axis.Pollux.Identity.Models
{
    public class UserGroup: BaseModel<Guid>
    {
        public string Name { get; set; }

        public User[] Members { get; set; }

        public int Status { get; set; }
    }
}
