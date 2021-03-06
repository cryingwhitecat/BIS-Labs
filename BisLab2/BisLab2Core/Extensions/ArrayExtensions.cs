using System;
using System.Linq;

namespace Core.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Slice<T>(this T[] source, int index, int length)
        {
            T[] slice = new T[length > source.Length ? source.Length : length];
            Array.Copy(source, index, slice, 0, length > (source.Length - index) ? (source.Length - index) : length);
            return slice;
        }

        public static string GetHexString(this byte[] input)
        {
            return string.Join(" ", input.Select(x => x.ToString("X2")));
        }
    }
}
