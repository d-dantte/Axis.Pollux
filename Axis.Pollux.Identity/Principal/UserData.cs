using System;
using Axis.Luna.Utils;
using Axis.Luna.Operation;

namespace Axis.Pollux.Identity.Principal
{
    public class UserData: PolluxModel<long>, IDataItem
    {
        public string Data { get; set; }

        public string Name { get; set; }

        public CommonDataType Type { get; set; }


        #region Navigation Properties
        public virtual User Owner { get; set; }
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

                case CommonDataType.DateTime: return ResolvedOp.Try(() => DateTime.Parse(Data).ToString()).Resolve();

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
