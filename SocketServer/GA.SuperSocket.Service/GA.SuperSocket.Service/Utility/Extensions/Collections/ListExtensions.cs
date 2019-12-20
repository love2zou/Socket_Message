using System.Collections;
using System.Collections.Generic;

namespace Globalegrow.Toolkit
{
    public static class ListExtensions
    {
        public static void AddRange(this IList list, params object[] items)
        {
            foreach (object item in items)
            {
                list.Add(item);
            }
        }

        public static void AddRange<T>(this IList list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                list.Add(item);
            }
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                list.Add(item);
            }
        }

        public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                if (list.Contains(item))
                {
                    list.Remove(item);
                }
            }
        }
    }
}