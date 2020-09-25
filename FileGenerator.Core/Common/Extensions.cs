using System;
using System.Collections.Generic;
using System.IO;

namespace FileGenerator.Core.Common
{
    public static class Extensions
    {
        public static char[] Append(this char[] destination, char[] source)
        {
            var initialSize = destination.Length;
            Array.Resize(ref destination, source.Length + destination.Length);
            Buffer.BlockCopy(source, 0, destination, initialSize * 2, source.Length * 2);
            return destination;
        }

        public static string Combine(this string source, string target)
        {
            return Path.Combine(source, target);
        }

        public static bool HasSingle<T>(this IEnumerable<T> sequence, out T value)
        {
            if (sequence is IList<T> list)
            {
                if (list.Count == 1)
                {
                    value = list[0];
                    return true;
                }
            }
            else
            {
                using var enumerator = sequence.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    value = enumerator.Current;
                    if (!enumerator.MoveNext())
                    {
                        return true;
                    }
                }
            }

            value = default(T);
            return false;
        }
    }
}