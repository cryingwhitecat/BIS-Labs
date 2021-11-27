using Core.Abstractions;
using System.Security.Cryptography;

namespace Lab4.Hashing
{
    public class Sha1HashAlgorithm : IHashingAlgorithm
    {
        public byte[] GetHash(byte[] input)
        {
            using (var sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(input);
            }
        }
    }
}
