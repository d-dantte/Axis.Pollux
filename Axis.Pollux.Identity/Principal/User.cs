using Axis.Pollux.Common;
using System;

namespace Axis.Pollux.Identity.Principal
{
    public class User: PolluxModel<long>
    {

        public User()
        {
        }
        
        public string UserName { get; set; }

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

        public Guid UId { get; set; } = Guid.NewGuid();


        public BioData BioData { get; set; }
        public ContactData[] ContactData { get; set; }
        public UserData[] UserData { get; set; }
    }
}