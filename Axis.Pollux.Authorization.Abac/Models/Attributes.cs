
namespace Axis.Pollux.Authorization.Abac.Models
{
    /// <summary>
    /// Defines well-known attributes within the system
    /// </summary>
    public static class ResourceAttributes
    {
        /// <summary>
        /// Value used to prefix attributes generated from the <c>IDataAccessDescriptor.DataDescriptors()</c> method
        /// </summary>
        public static readonly string DataDescriptorPrefix = "Pollux.DataAccess.Descriptor";

        /// <summary>
        /// A resource attribute used for OperationAccess policy resolution. It is the serialized version of the <c>OperationAccessDescriptor</c>
        /// object passed into the <c>IOperationAccessAthorizer</c> service. The policy is meant to use this object, along with any parameters it may contain,
        /// to make it's authorization decision
        /// </summary>
        public static readonly string OperationAccessDescriptor = "Pollux.OperationAccess.Descriptor";
    }}
