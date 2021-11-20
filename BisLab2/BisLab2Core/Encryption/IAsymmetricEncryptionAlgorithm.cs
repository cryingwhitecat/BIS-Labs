using System.IO;

namespace Core.Encryption
{
    public interface IAsymmetricEncryptionAlgorithm
    {
        void Encrypt(Stream inputStream, Stream outputStream);
        string Decrypt(Stream inputStream);
    }
}
