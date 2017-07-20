using Axis.Pollux.Common;
using System;

namespace Axis.Pollux.Identity.OAModule.Entities
{
    public class UserEntity : PolluxEntity<string>
    {

        public UserEntity()
        {
            GUId = Guid.NewGuid();
        }

        /// <summary>
        /// Account Status
        /// <para>
        /// The value of this property should depict the status of the User in the system.
        /// The actual values, and the intepretation of the values is left to the discretion
        /// of the system in which it is used.
        /// </para>
        /// <para>
        /// Pollux does not provide any services that utilizes (i.e validates) this value,
        /// and as such, the final system that will need to use the value will have to 
        /// provide that service.
        /// </para>
        /// <para>
        /// Possible values include: Active, Suspended, Blocked, etc...
        /// </para>
        /// </summary>
        public int Status { get; set; }

        public Guid GUId { get; set; }
    }
}
