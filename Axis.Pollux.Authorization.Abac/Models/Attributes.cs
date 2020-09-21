
using Axis.Sigma;

namespace Axis.Pollux.Authorization.Abac.Models
{
    /// <summary>
    /// Defines well-known attributes within the system
    /// </summary>
    public static class ResourceAttributeNames
    {
        /// <summary>
        /// Attribute representing the data type of the data being accessed
        /// </summary>
        public static readonly string DataAccessType = "Pollux.DataAccess.Resource.DataType";

        /// <summary>
        /// Attribute representing the unique id of the data being accessed
        /// </summary>
        public static readonly string DataAccessId = "Pollux.DataAccess.Resource.Id";

        /// <summary>
        /// A resource attribute used for OperationAccess policy resolution. It identifies the name of the operation being Invoked,
        /// to make it's authorization decision
        /// </summary>
        public static readonly string OperationAccessName = "Pollux.OperationAccess.Resource.Name";

        /// <summary>
        /// A resource attribute used for OperationAccess policy resolution. It represents a parameter being passed into the operation being Invoked.
        /// This attribute is optional
        /// </summary>
        public static readonly string OperationAccessParameterPrefix = "Pollux.OperationAccess.Resource.Param";
    }

    public static class IntentAttributeNames
    {
        /// <summary>
        /// Specifies the type of access to be performed on the operation. Values is always "Invoke"
        /// </summary>
        public static readonly string OperationIntent = "Pollux.OperationAccess.Intent.AccessType";

        /// <summary>
        /// Specifies the type of access to be performed on the data
        /// </summary>
        public static readonly string DataAccessIntent = "Pollux.DataAccess.Intent.AccessType";
    }


    public static class DefaultIntentAttributes
    {
        /// <summary>
        /// </summary>
        public static readonly IAttribute OperationIntent = new Attribute(AttributeCategory.Intent)
        {
            Name = IntentAttributeNames.OperationIntent,
            Type = Luna.Common.CommonDataType.String,
            Data = "Invoke"
        };
    }
}
