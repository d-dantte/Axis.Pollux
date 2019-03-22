using static Axis.Luna.Extensions.TypeExtensions;

using System;
using Axis.Pollux.Authorization.Models;
using System.Collections.Generic;

namespace Axis.Pollux.Authentication.Services.AccessDescriptors
{
    /// <summary>
    /// 
    /// </summary>
    public class UserOwnedData : IDataAccessDescriptor
    {
        public string RootType => typeof(UserOwnedData).MinimalAQName();

        /// <summary>
        /// A unique string label signifying the data being accessed. Typically, this will be Type.FullName (or Type.MinimalAQName), but can be anything at all.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Identifying the object/data to which access is sought.
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// A unique value signifying that access to a specific object is requested.
        /// </summary>
        public Guid? OwnerId { get; set; }

        public IEnumerable<DataAttribute> DataDescriptors()
        {
            var list = new List<DataAttribute>();

            if (!string.IsNullOrWhiteSpace(DataType)) list.Add(new DataAttribute
            {
                Type = Luna.Common.CommonDataType.String,
                Name = nameof(DataType),
                Data = DataType
            });

            if (!string.IsNullOrWhiteSpace(DataId)) list.Add(new DataAttribute
            {
                Type = Luna.Common.CommonDataType.String,
                Name = nameof(DataId),
                Data = DataId
            });

            if (OwnerId != null) list.Add(new DataAttribute
            {
                Type = Luna.Common.CommonDataType.Guid,
                Name = nameof(OwnerId),
                Data = OwnerId?.ToString() ?? null
            });

            return list;
        }
    }
}
