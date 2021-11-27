using System.IO;

namespace Core.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            stream.Position = 0;
            var inputLength = stream.Length;
            var input = new byte[inputLength];
            stream.Read(input, 0, (int)inputLength);
            return input;
        }
    }
}
