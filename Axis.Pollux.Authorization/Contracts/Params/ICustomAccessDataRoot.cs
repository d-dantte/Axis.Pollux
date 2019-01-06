using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Authorization.Contracts.Params
{
    /// <summary>
    /// Custom data-access authorization uses this as a root to encapsulate other data that a specific policy will evaluate.
    /// 
    /// </summary>
    public interface ICustomAccessDataRoot: IValidatable
    {
        string CustomDataType { get; }

        object CompressObjectGraph();
    }
}
