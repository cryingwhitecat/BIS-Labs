using System.IO;

namespace Core.Abstractions
{
    public interface IEncryptionAlgorithm
    {
        void Encrypt(Stream inputStream, Stream outputStream);
        void Decrypt(Stream inputStream, Stream outputStream);
    }
}
