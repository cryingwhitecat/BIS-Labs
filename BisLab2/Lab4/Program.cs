using Core.Extensions;
using Lab4.Encryption;
using Lab4.Hashing;
using Rsa.Encryption;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Lab4
{
    class Program
    {
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory + "../../../";

        static void Main(string[] args)
        {
            var hashing = new Sha1HashAlgorithm();
            var permutationTableLength = 10 + (24 + 82) % 7;
            var random = new Random();
            var cert = new X509Certificate2(Path.Combine(basePath, "Keys/cert.pfx"), "1234");
            var rsa = new RsaAlgorithm(cert);
            var permutationTable = Enumerable.Range(0, permutationTableLength).Select(x => (byte)x).Reverse();
            var symmetricAlg = new PermutationTableAlgorithm(permutationTable);
            var message = Encoding.UTF8.GetBytes("hello world");
            var hash = hashing.GetHash(message);
            Console.WriteLine($"Init hash: {hash.GetHexString()}");
            var messageStream = new MemoryStream(message);
            var encryptedMessage = new MemoryStream();
            symmetricAlg.Encrypt(messageStream, encryptedMessage);

            var permutationTableStream = new MemoryStream(permutationTable.ToArray());
            var encryptedPermutationTable = new MemoryStream();

            rsa.Encrypt(permutationTableStream, encryptedPermutationTable);

            var decryptedPermutationTable = new MemoryStream();
            encryptedPermutationTable.Position = 0;
            rsa.Decrypt(encryptedPermutationTable, decryptedPermutationTable);

            var newSymmetricAlg = new PermutationTableAlgorithm(decryptedPermutationTable.ToArray());
            var decryptedMessageStream = new MemoryStream();
            newSymmetricAlg.Decrypt(encryptedMessage, decryptedMessageStream);
            var decryptedMessage = decryptedMessageStream.ToArray();
            var decryptedMessageHash = new Sha1HashAlgorithm().GetHash(decryptedMessage);
            Console.WriteLine($"Decrypted message: {Encoding.UTF8.GetString(decryptedMessage)}");
            Console.WriteLine($"Decrypted message hash: {decryptedMessageHash.GetHexString()}");
        }
    }
}
