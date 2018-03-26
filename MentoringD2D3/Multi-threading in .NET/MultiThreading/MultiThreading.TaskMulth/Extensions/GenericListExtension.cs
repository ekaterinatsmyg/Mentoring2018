using System;
using System.Collections.Generic;
using System.Text;

namespace MultiThreading.TaskMulth.Extensions
{
    public static class GenericListExtension
    {
        /// <summary>
        /// Converts enumeration to array if a finit number is knownin order to safe space in the heap.
        /// </summary>
        /// <typeparam name="TSource">A type of elements in the enumeration.</typeparam>
        /// <param name="source">The original enumeration.</param>
        /// <param name="count">The finit number of elements in the resulting array.</param>
        /// <returns>The resulting array.</returns>
        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            //TODO : Verifiation if source.Count less than count 

            var array = new TSource[count];
            int i = 0;

            foreach (var item in source)
            {
                array[i++] = item;
            }

            if (i != count) throw new ArgumentOutOfRangeException();
            return array;
        }

        /// <summary>
        /// Provides a string representaion of enumeration.
        /// </summary>
        /// <typeparam name="TSource">A type of elements in the enumeration.</typeparam>
        /// <param name="source">The original enumeration.</param>
        /// <param name="seperator">The string representation of the source.</param></param>
        /// <returns></returns>
        public static string ToString<TSource>(this IEnumerable<TSource> source, string seperator)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    return String.Empty;
                }

                var builder = new StringBuilder().Append(enumerator.Current);

                while (enumerator.MoveNext())
                {
                    builder.Append(seperator).Append(enumerator.Current);
                }

                return builder.ToString();
            }
        }
    }
}
