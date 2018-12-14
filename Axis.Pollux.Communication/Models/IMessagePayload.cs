using Axis.Pollux.Common.Models;

namespace Axis.Pollux.Communication.Models
{
    public interface IMessagePayload: IValidatable
    {
        string PayloadType { get; set; }
    }
}
