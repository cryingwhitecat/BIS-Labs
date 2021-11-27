using BisLab2.Core;
using BisLab2.Core.KeyTransformers;
using BisLab2.Des.Encryption;
using Core.Abstractions;
using System;
using System.IO;
using System.Text;

namespace Core
{
    class Program
    {
        private static KeyTransformer keyTransformer = new KeyTransformer();
        private static IKeyedEncryptionAlgorithm desEncryptor = new DesAlgorithm();
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "../../../";
        private static string encryptedPath = "Texts/des-encrypted.hex";
        private static string plaintextPath = "Texts/plaintext.hex";
        static void Main(string[] args)
        {
            var password = "Руденко Давид Сергійович";
            //Console.Write("Enter your password: ");
            //password = Console.ReadLine();
            var key = keyTransformer.GetKey(password, 8);

            Console.WriteLine($"Encryption key is: {key.ToHexString()}");
            var input = Encoding.ASCII.GetBytes(File.ReadAllText(Path.Combine(basePath, plaintextPath)));
            using(var fileStream = File.Open(Path.Combine(basePath, encryptedPath), FileMode.OpenOrCreate))
            {
                desEncryptor.Encrypt(input, key, fileStream);
                Console.WriteLine($"Encrypted file saved at {encryptedPath}");
            }
            using (var decryptStream = File.Open(Path.Combine(basePath, encryptedPath), FileMode.Open))
            {
                var decrypted = desEncryptor.Decrypt(decryptStream, key);
                Console.Write($"Decrypted text: {decrypted}");
            }
        }
    }
}
