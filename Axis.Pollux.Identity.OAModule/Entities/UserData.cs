using Axis.Luna.Utils;

namespace Axis.Pollux.Identity.OAModule.Entities
{
    public class UserDataEntity : PolluxEntity<long>, IDataItem
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public CommonDataType Type { get; set; }


        #region Navigation Properties
        public virtual UserEntity Owner { get; set; }
        public string OwnerId { get; set; }
        #endregion


        public UserDataEntity()
        {
        }
    }
}
