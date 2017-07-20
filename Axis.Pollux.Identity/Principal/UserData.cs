using Axis.Luna.Utils;
using Axis.Pollux.Common;

namespace Axis.Pollux.Identity.Principal
{
    public class UserData: PolluxModel<long>, IDataItem, IUserOwned
    {
        public string Data
        {
            get { return _dataItem.Data; }
            set { _dataItem.Data = value; }
        }

        public string Name
        {
            get { return _dataItem.Name; }
            set { _dataItem.Name = value; }
        }

        public CommonDataType Type
        {
            get { return _dataItem.Type; }
            set { _dataItem.Type = value; }
        }

        /// <summary>
        /// Status of the data. Meaning is given to this field by the application using the data.
        /// It can be used to implement different status for the various kinds of data stored in here: Active, Enabled,
        /// Inactive, Deprecated, Expired, etc
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// User defined label for this data. A means to classify/customize/name this data by the user
        /// </summary>
        public string Label { get; set; }


        #region Navigation Properties
        public virtual User Owner { get; set; }
        #endregion

        public override string ToString() => _dataItem.ToString();


        private DataItem _dataItem = new DataItem();
        public UserData()
        {
        }
    }
}
