using BisLab2.Core;
using BisLab2.Core.KeyTransformers;
using Core.Abstractions;
using System;
using System.IO;
using System.Text;
using TripleDes.Encryption;

namespace BisLab2TripleDes
{
    class Program
    {
        private static KeyTransformer keyTransformer = new KeyTransformer();
        private static IKeyedEncryptionAlgorithm desEncryptor = new TripleDesAlgorithm();
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "../../../";
        private static string encryptedPath = "Texts/triple-des-encrypted.hex";
        private static string plaintextPath = "Texts/plaintext.txt";
        static void Main(string[] args)
        {
            var password = "Руденко Давид Сергійович";
            //Console.Write("Enter your password: ");
            //password = Console.ReadLine();
            var key = keyTransformer.GetKey(password, 24);

            Console.WriteLine($"Encryption key is: {key.ToHexString()}");
            var input = Encoding.ASCII.GetBytes(File.ReadAllText(Path.Combine(basePath, plaintextPath)));
            using (var fileStream = File.Open(Path.Combine(basePath, encryptedPath), FileMode.OpenOrCreate))
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
