using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Globalegrow.Toolkit
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || enumerable.Count() < 1;
        }

        public static string ToCommaSeparatedList(this IEnumerable<string> enumerable)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in enumerable)
            {
                sb.Append(s);
                sb.Append(',');
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        public static string ToCommaSeparatedList<T>(this IEnumerable<T> enumerable)
        {
            StringBuilder sb = new StringBuilder();

            foreach (T item in enumerable)
            {
                sb.Append(item);
                sb.Append(',');
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }

    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;

        public CommonEqualityComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }
}