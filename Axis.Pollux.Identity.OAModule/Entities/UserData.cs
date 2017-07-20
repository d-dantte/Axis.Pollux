using Axis.Luna.Utils;
using Axis.Pollux.Common;

namespace Axis.Pollux.Identity.OAModule.Entities
{
    public class UserDataEntity : PolluxEntity<long>, IDataItem
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public CommonDataType Type { get; set; }

        public int Status { get; set; }

        public string Label { get; set; }


        #region Navigation Properties
        public virtual UserEntity Owner { get; set; }
        public string OwnerId { get; set; }
        #endregion


        public UserDataEntity()
        {
        }
    }
}
