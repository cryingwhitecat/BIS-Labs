using System.IO;

namespace Core.Abstractions
{
    public interface IKeyedEncryptionAlgorithm
    {
        void Encrypt(byte[] input, byte[] key, Stream outputStream);
        string Decrypt(Stream inputStream, byte[] key);
    }
}
