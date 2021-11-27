using Core.Abstractions;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab4.Encryption
{
    public class PermutationTableAlgorithm : IEncryptionAlgorithm
    {
        private readonly byte[] permutations;
        private readonly int permutationTableLength;
        public byte[] Permutations { get { return permutations; } }
        public PermutationTableAlgorithm(IEnumerable<byte> encryptedPositions)
        {
            permutations = encryptedPositions.ToArray();
            permutationTableLength = permutations.Length;
        }

        public void Decrypt(Stream inputStream, Stream outputStream)
        {
            var input = inputStream.ReadAllBytes();
            var inputLength = input.Length;
            var output = new byte[inputLength];
            for (int i = 0; i < inputLength; i++)
            {
                var outputPos = Array.FindIndex(permutations, x => x == i % permutationTableLength);
                output[outputPos] = input[i];
            }
            outputStream.Write(output, 0, output.Length);
        }

        public void Encrypt(Stream inputStream, Stream outputStream)
        {
            var input = inputStream.ReadAllBytes();
            var inputLength = input.Length;
            var output = new byte[inputLength];
            for(int i = 0; i < inputLength; i++)
            {
                var outputPos = permutations[i % permutationTableLength];
                output[outputPos] = input[i];
            }
            outputStream.Write(output, 0, output.Length);
        }
    }
}
