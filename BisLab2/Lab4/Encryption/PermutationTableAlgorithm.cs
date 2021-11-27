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
            byte[] result = null;
            var blocksCount = inputLength / permutationTableLength;
            blocksCount = blocksCount == 0 ? 1 : blocksCount;
            for (int i = 0; i < blocksCount; i++)
            {
                var block = input.Skip(i * permutationTableLength).Take(permutationTableLength).ToArray();
                block = block.Concat(new byte[permutationTableLength - block.Length]).ToArray();
                var encryptedBlock = new byte[permutationTableLength];
                for (int j = 0; j < block.Length; j++)
                {
                    var encryptedPos = permutations[j];
                    encryptedBlock[encryptedPos] = block[j];
                }
                result = result == null ? encryptedBlock : result.Concat(encryptedBlock).ToArray();
            }
            outputStream.Write(result, 0, result.Length);
        }

        public void Encrypt(Stream inputStream, Stream outputStream)
        {
            var input = inputStream.ReadAllBytes();
            var inputLength = input.Length;
            byte[] result = null;
            var blocksCount = inputLength / permutationTableLength;
            blocksCount = blocksCount == 0 ? 1 : blocksCount;
            for (int i = 0; i <= blocksCount; i++)
            {
                var block = input.Skip(i * permutationTableLength).Take(permutationTableLength).ToArray();
                block = block.Concat(new byte[permutationTableLength - block.Length]).ToArray();
                var encryptedBlock = new byte[permutationTableLength];
                for (int j = 0; j < permutationTableLength; j++)
                {
                    var encryptedPos = permutations[j];
                    encryptedBlock[j] = block[encryptedPos % block.Length];
                }
                if (result == null)
                {
                    result = encryptedBlock;
                }
                else
                {
                    result = result.Concat(encryptedBlock).ToArray();
                }
            }
            outputStream.Write(result, 0, result.Length);
        }
    }
}
