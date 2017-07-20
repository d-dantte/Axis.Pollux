using Axis.Pollux.Common;
using Axis.Pollux.Identity.Principal;
using System;

namespace Axis.Pollux.Identity.OAModule.Entities
{
    public class BioDataEntity : PolluxEntity<long>
    {
        #region Names
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        #endregion

        #region Bio
        public DateTime? Dob { get; set; }
        public Gender Gender { get; set; }

        public string Nationality { get; set; }
        public string StateOfOrigin { get; set; }
        #endregion

        #region navigational properties
        public virtual UserEntity Owner { get; set; }
        public string OwnerId { get; set; }
        #endregion

        public BioDataEntity()
        {
        }
    }
}
