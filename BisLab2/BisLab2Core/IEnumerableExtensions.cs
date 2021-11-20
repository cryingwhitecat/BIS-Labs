using System;
using System.Collections.Generic;
using System.Linq;
namespace BisLab2.Core
{
    public static class IEnumerableExtensions
    {
        public static List<T> AppendToEnd<T>(this IEnumerable<T> collection, IEnumerable<T> toAdd)
        {
            var list = collection.ToList();
            list.AddRange(toAdd);
            return list;
        }

        public static string ToHexString(this byte[] array)
        {
            return string.Join(" ", Array.ConvertAll(array, x => x.ToString("X2")));
        }
    }
}
