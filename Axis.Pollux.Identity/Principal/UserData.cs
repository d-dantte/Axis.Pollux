using Axis.Luna;
using static Axis.Luna.Extensions.ObjectExtensions;
using System;

namespace Axis.Pollux.Identity.Principal
{
    public class UserData: PolluxEntity<long>, IDataItem
    {
        public string Data
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public string Name
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        public CommonDataType Type
        {
            get { return get<CommonDataType>(); }
            set { set(ref value); }
        }


        #region Navigation Properties
        public virtual User Owner  { get { return get<User>(); } set { set(ref value); } }
        public virtual string OwnerId  { get { return get<string>(); } set { set(ref value); } }
        #endregion


        public override string ToString() => $"[{Name}: {DisplayData()}]";

        private string DisplayData()
        {
            switch (Type)
            {
                case CommonDataType.Boolean:
                case CommonDataType.Real:
                case CommonDataType.Integer:
                case CommonDataType.String:
                case CommonDataType.Url:
                case CommonDataType.TimeSpan:
                case CommonDataType.Email:
                case CommonDataType.Phone:
                case CommonDataType.Location:
                case CommonDataType.IPV4:
                case CommonDataType.IPV6:
                case CommonDataType.JsonObject: return Data;

                case CommonDataType.DateTime: return Eval(() => DateTime.Parse(Data).ToString(), ex => "");

                case CommonDataType.Binary: return "Binary-Data";

                case CommonDataType.UnknownType:
                default: return "Unknown-Type";
            }
        }


        public UserData()
        {
        }
    }
}
