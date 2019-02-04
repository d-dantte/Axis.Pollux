using System;
using System.Collections.Generic;
using System.Text;
using Axis.Luna.Common;
using Axis.Luna.Common.Contracts;
using Axis.Luna.Extensions;
using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Identity.Models
{
    public class UserData: BaseModel<Guid>, IUserOwned, IDataItem
    {
        public string DisplayData()
        {
            var display = $"{Name}: ";
            switch (Type)
            {
                case CommonDataType.Boolean:
                case CommonDataType.Real:
                case CommonDataType.Decimal:
                case CommonDataType.Integer:
                case CommonDataType.String:
                case CommonDataType.Url:
                case CommonDataType.TimeSpan:
                case CommonDataType.Email:
                case CommonDataType.Phone:
                case CommonDataType.Location:
                case CommonDataType.IPV4:
                case CommonDataType.IPV6:
                case CommonDataType.DateTime:
                case CommonDataType.CSV:
                case CommonDataType.NVP:
                case CommonDataType.Guid:
                case CommonDataType.JsonObject: return Data;

                case CommonDataType.Binary: return "Binary-Data";

                case CommonDataType.UnknownType:
                default: return "Unknown-Type";
            }
        }

        public void Initialize(string[] tuples)
        {
            Name = tuples[0];
            Enum.TryParse(tuples[1], true, out CommonDataType cdt)
                .ThrowIf(false, $"Invalid Common Data Type: {tuples[1]}");
            Type = cdt;
            Status = int.Parse(tuples[2]);
            Data = tuples[3];
        }

        public string[] Tupulize()
        => new []
        {
            Name,
            Type.ToString(),
            Status.ToString(),
            Data
        };

        public string Name { get; set; }
        public string Data { get; set; }
        public CommonDataType Type { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// Context-based String labels
        /// </summary>
        public string[] Tags { get; set; }

        public User Owner { get; set; }
    }

    /// <summary>
    /// Possible status values for the user
    /// </summary>
    public enum UserDataStatus
    {
        Archived = 0,
        Active = 1
    }
}
