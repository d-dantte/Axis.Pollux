using static Axis.Luna.Extensions.ObjectExtensions;

using System;
using System.Security.Cryptography;
using System.Text;
using Axis.Pollux.Authentication;
using System.Threading;

namespace Axis.Pollux.CoreAuthentication
{
    public class DefaultHasher : ICredentialHasher
    {
        private SHA256 hasher = SHA256.Create();

        public string CalculateHash(string data) => Encode(RawHash(data));
        public string CalculateHash(byte[] data) => Encode(RawHash(data));

        public bool IsValidHash(string data, string hash)
            => (RawHash(data) == Decode(hash)).UsingValue(bytes => Thread.Sleep(2000)); //<-- waste some time to discourage brute force attacks

        public bool IsValidHash(byte[] data, string hash)
            => (RawHash(data) == Decode(hash)).UsingValue(bytes => Thread.Sleep(2000)); //<-- waste some time to discourage brute force attacks

        /// <summary>
        /// Alter the hash with some reversible encryption
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        private string Encode(string hash) => hash;

        /// <summary>
        /// Decrypt the hash
        /// </summary>
        /// <param name="encodedHash"></param>
        /// <returns></returns>
        private string Decode(string encodedHash) => encodedHash;

        private string RawHash(string data)
            => Encoding.Unicode.GetBytes(data)
                       .Pipe(bytes => Convert.ToBase64String(hasher.ComputeHash(bytes)));

        private string RawHash(byte[] data)
            => Convert.ToBase64String(hasher.ComputeHash(data));
    }
}
