using Axis.Luna.Operation;

namespace Axis.Pollux.Authentication.Contracts
{
    public interface ICredentialHasher
    {
        Operation<string> CalculateHash(string data);
        Operation<string> CalculateHash(byte[] data);
        Operation ValidateHash(string data, string hash);
        Operation ValidateHash(byte[] data, string hash);
    }
}
