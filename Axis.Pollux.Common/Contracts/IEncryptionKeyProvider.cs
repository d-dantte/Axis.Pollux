using Axis.Luna.Operation;
using Axis.Pollux.Common.Contracts.Params;

namespace Axis.Pollux.Common.Contracts
{
    public interface IEncryptionKeyProvider
    {
        Operation<string> GetOrGenerateKey(string keyId);

        Operation<KeyPair> GetOrGenerateKeyPair(string keyId);
    }
}
