using System;
using System.Collections.Generic;
using System.Text;

namespace Axis.Pollux.Authorization.Abac.Models
{
    /// <summary>
    /// Defines well-known attributes within the system
    /// </summary>
    public static class ResourceAttributes
    {
        /// <summary>
        /// A resource attribute used for DataAccess policy resolution, signifying the type of the data being accessed.
        /// It is typically the fully qualified name of the c# type, devoid of the assembly information.
        /// </summary>
        public static readonly string DataAccessDataType = "Pollux.DataAccess.DataType";

        /// <summary>
        /// A resource attribute used for DataAccess policy resolution, signifying the Id of the PRINCIPAL
        /// that owns the data - meaning access is sought for any/all data of the specified type belonging
        /// to the specified PRINCIPAL. If not present, signifying access to all data of the specified type.
        /// </summary>
        public static readonly string DataAccessOwnerId = "Pollux.DataAccess.OwnerId";

        /// <summary>
        /// A resource attribute used for DataAccess policy resolution, signifying a string version of some
        /// Uniquely identifying property of its own (typically, it's key). When not present, any data is
        /// sought.
        /// </summary>
        public static readonly string DataAccessUniqueId = "Pollux.DataAccess.UniqueId";

        /// <summary>
        /// A resource attribute used for OperationAccess policy resolution, signifying a unique label that
        /// describes the operation to which access is sought.
        /// </summary>
        public static readonly string OperationAccessDescriptor = "Pollux.OperationAccess.Descriptor";
    }}
