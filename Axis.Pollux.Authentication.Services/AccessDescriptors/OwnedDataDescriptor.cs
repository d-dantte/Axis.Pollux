using static Axis.Luna.Extensions.TypeExtensions;

using Axis.Luna.Operation;
using Axis.Pollux.Identity.Exceptions;
using System;
using Axis.Pollux.Authorization.Models;

namespace Axis.Pollux.Authentication.Services.AccessDescriptors
{
    public class OwnedDataDescriptor : IDataAccessDescriptor
    {
        public string CustomDataType => typeof(OwnedDataDescriptor).MinimalAQName();

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

        public Operation Validate()
        => Operation.Try(() =>
        {
            if (string.IsNullOrWhiteSpace(DataType))
                throw new IdentityException(Common.Exceptions.ErrorCodes.ModelValidationError);
        });
    }
}
