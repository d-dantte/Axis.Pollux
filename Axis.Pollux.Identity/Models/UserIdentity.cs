using Axis.Pollux.Common.Models;
using System;

namespace Axis.Pollux.Identity.Models
{
    /// <summary>
    /// Encapsulates a Name that identifies a user within the system.
    /// Uniqueness of this feature is left to the implementation
    /// </summary>
    public class UserIdentity: BaseModel<Guid>, IUserOwned
    {
        public string Name { get; set; }

        public User Owner { get; set; }
    }
}
