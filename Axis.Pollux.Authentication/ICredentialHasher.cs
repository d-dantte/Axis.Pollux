
namespace Axis.Pollux.Authentication
{
    public interface ICredentialHasher
    {
        string CalculateHash(string data);
        string CalculateHash(byte[] data);
        bool IsValidHash(string data, string hash);
        bool IsValidHash(byte[] data, string hash);
    }
}
