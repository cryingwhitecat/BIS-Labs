using Rsa.Encryption;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Rsa
{
    class Program
    {
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "../../../";
        static void Main(string[] args)
        {
            var cert = new X509Certificate2(Path.Combine(basePath, "Keys/cert.pfx"), "1234");
            var rsa = new RsaAlgorithm(cert);
            using (var inputFile = File.Open(Path.Combine(basePath, "Texts/plaintext.txt"), FileMode.Open))
            {
                using (var outputFile = File.Open(Path.Combine(basePath, "Texts/encrypted.hex"), FileMode.OpenOrCreate))
                {
                    rsa.Encrypt(inputFile, outputFile);
                }
            }
            using (var outputFile = File.Open(Path.Combine(basePath, "Texts/encrypted.hex"), FileMode.OpenOrCreate))
            {
                var decryptedStream = new MemoryStream();
                rsa.Decrypt(outputFile, decryptedStream);
                Console.WriteLine($"decrypted text: {Encoding.UTF8.GetString(decryptedStream.ToArray())}");
            }
        }
    }
}
