using System.IO;

namespace Core.Encryption
{
    public interface IEncryptionAlgorithm
    {
        void Encrypt(byte[] input, byte[] key, Stream outputStream);
        string Decrypt(Stream inputStream, byte[] key);
    }
}
