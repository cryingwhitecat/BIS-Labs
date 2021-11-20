using Core.Encryption;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Rsa.Encryption
{
    class RsaAlgorithm : IAsymmetricEncryptionAlgorithm
    {
        private readonly X509Certificate2 keysCertificate;
        private readonly RSAEncryptionPadding encryptionPadding;
        public RsaAlgorithm(X509Certificate2 keysCertificate)
        {
            this.keysCertificate = keysCertificate;
            encryptionPadding = RSAEncryptionPadding.Pkcs1;
        }

        public string Decrypt(Stream inputStream)
        {
            var input = ReadAllBytes(inputStream);
            var rsa = keysCertificate.GetRSAPrivateKey();
            var decrypted = rsa.Decrypt(input, encryptionPadding);
            return Encoding.UTF8.GetString(decrypted);
        }

        public void Encrypt(Stream inputStream, Stream outputStream)
        {
            var input = ReadAllBytes(inputStream);
            var rsa = keysCertificate.GetRSAPublicKey();
            var encrypted = rsa.Encrypt(input, encryptionPadding);
            outputStream.Write(encrypted, 0, encrypted.Length);
        }

        private byte[] ReadAllBytes(Stream stream)
        {
            var inputLength = stream.Length;
            var input = new byte[inputLength];
            stream.Read(input, 0, (int)inputLength);
            return input;
        }
    }
}
