using Axis.Pollux.Common;
using System;

namespace Axis.Pollux.Identity.OAModule.Entities
{
    public class CorporateDataEntity : PolluxEntity<long>
    {
        #region Identity
        public string CorporateName { get; set; }
        public string CorporateId { get; set; }
        #endregion

        #region Misc
        public string Description { get; set; }
        public DateTime? IncorporationDate { get; set; }

        public int Status { get; set; }
        #endregion

        #region navigational properties
        public virtual UserEntity Owner { get; set; }
        public string OwnerId { get; set; }
        #endregion
    }
}
