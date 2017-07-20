using Axis.Pollux.Common;
using System;

namespace Axis.Pollux.Identity.Principal
{
    public class CorporateData: PolluxModel<long>, IUserOwned
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
        public virtual User Owner { get; set; }
        #endregion
    }
}
