using Axis.Luna.Operation;

namespace Axis.Pollux.Communication.Contracts
{
    public interface ISystemChannelSourceAddressProvider
    {
        Operation<string> GetChannelSourceAddress(string channel, string messagePayloadType);
    }
}
