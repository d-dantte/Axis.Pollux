using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Authorization.Models
{
    /// <summary>
    /// data-access authorization uses this as a root to encapsulate other data that a specific policy will evaluate.
    /// 
    /// </summary>
    public interface IDataAccessDescriptor: IValidatable
    {
        string CustomDataType { get; }
    }
}
