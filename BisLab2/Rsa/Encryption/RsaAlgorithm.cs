using Core.Abstractions;
using Core.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Rsa.Encryption
{
    public class RsaAlgorithm : IEncryptionAlgorithm
    {
        private readonly X509Certificate2 keysCertificate;
        private readonly RSAEncryptionPadding encryptionPadding;
        private readonly int encryptBlockSize;
        private readonly int decryptBlockSize;
        public RsaAlgorithm(X509Certificate2 keysCertificate)
        {
            this.keysCertificate = keysCertificate;
            encryptionPadding = RSAEncryptionPadding.Pkcs1;
            decryptBlockSize = this.keysCertificate.PrivateKey.KeySize / 8;
            encryptBlockSize = this.keysCertificate.PrivateKey.KeySize / 8 - 11;
        }

        public void Decrypt(Stream inputStream, Stream outputStream)
        {
            var input = inputStream.ReadAllBytes();
            int blockIndex = 0;
            int blockCount = input.Length / decryptBlockSize;
            if (blockCount == 0)
                blockCount = 1;
            var decrypted = new List<byte>();
            using(var rsa = keysCertificate.GetRSAPrivateKey())
            {
                for (; blockIndex < blockCount; blockIndex++)
                {
                    byte[] block = input.Skip(blockIndex * decryptBlockSize).Take(decryptBlockSize).ToArray();
                    var decryptedBlock = rsa.Decrypt(block, encryptionPadding);
                    decrypted.AddRange(decryptedBlock);
                }
            }
            outputStream.Write(decrypted.ToArray(), 0, decrypted.Count);
        }

        public void Encrypt(Stream inputStream, Stream outputStream)
        {
            var input = inputStream.ReadAllBytes();
            using (var rsa = keysCertificate.GetRSAPublicKey())
            {
                int blockIndex = 0;
                int blockCount = input.Length / encryptBlockSize;
                if (blockCount == 0)
                    blockCount = 1;
                for (; blockIndex < blockCount; blockIndex++)
                {
                    byte[] block = input.Skip(blockIndex * encryptBlockSize).Take(encryptBlockSize).ToArray();
                    var encryptedBlock = rsa.Encrypt(block, encryptionPadding);
                    outputStream.Write(encryptedBlock, 0, encryptedBlock.Length);
                }
            }
        }

    }
}
