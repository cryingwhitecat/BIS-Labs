using Core.Encryption;
using System.IO;
using System.Security.Cryptography;

namespace BisLab2.Des.Encryption
{
    internal class DesAlgorithm: IEncryptionAlgorithm
    {
        public void Encrypt(byte[] input, byte[] key, Stream outputStream)
        {
            var des = InitDesObject(key);
            using (var cryptoStream = new CryptoStream(outputStream, des.CreateEncryptor(des.Key, des.IV), CryptoStreamMode.Write))
            {
                cryptoStream.Write(input, 0, input.Length);
            }
        }

        public string Decrypt(Stream inputStream, byte[] key)
        {
            var des = InitDesObject(key);
            using(var cryptoStream = new CryptoStream(inputStream, des.CreateDecryptor(des.Key, des.IV), CryptoStreamMode.Read))
            {
                using(var streamReader = new StreamReader(cryptoStream))
                {
                    var text = streamReader.ReadToEnd();
                    return text;
                }
            }
        }

        private DES InitDesObject(byte[] key)
        {
            var des = DES.Create();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            des.Key = key;
            return des;
        }
    }
}
