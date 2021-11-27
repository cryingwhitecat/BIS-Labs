using Core.Abstractions;
using System.IO;
using System.Security.Cryptography;

namespace TripleDes.Encryption
{
    class TripleDesAlgorithm : IKeyedEncryptionAlgorithm
    {
        public string Decrypt(Stream inputStream, byte[] key)
        {
            var tdes = InitTripleDes(key);
            using (var cryptoStream = new CryptoStream(inputStream, tdes.CreateDecryptor(tdes.Key, tdes.IV), CryptoStreamMode.Read))
            {
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    var text = streamReader.ReadToEnd();
                    return text;
                }
            }
        }

        public void Encrypt(byte[] input, byte[] key, Stream outputStream)
        {
            using (var tdes = InitTripleDes(key))
            {
                using (var cryptoStream = new CryptoStream(outputStream, 
                        tdes.CreateEncryptor(tdes.Key, tdes.IV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(input, 0, input.Length);
                }
            }
        }

        private TripleDESCryptoServiceProvider InitTripleDes(byte[] key) 
        {
            var tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = key;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.Zeros;
            return tdes;
        }
    }
}
