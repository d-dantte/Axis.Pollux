using System.Collections.Generic;

namespace Axis.Pollux.Authorization.Models
{
    /// <summary>
    /// Encapsulates the individual data elements that describe the data to which access is needed.
    /// </summary>
    public interface IDataAccessDescriptor
    {
        /// <summary>
        /// Return all necessary attributes describing the data access scenario.
        /// </summary>
        /// <returns>All attributes from this function MUST be valid at the point of return</returns>
        IEnumerable<DataAttribute> DataDescriptors();
    }
}
