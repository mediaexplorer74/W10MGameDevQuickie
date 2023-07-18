using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Win8.Core.Tasks.Extensions
{
    /// <summary>
    /// Class containig various clooection extenstions.
    /// </summary>
    public static class CollectionExtensions
    {
        // IEnumerable Extensions

        /// <summary>
        /// Create an <see cref="ObservableCollection{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> source)
        {
            return new ObservableCollection<TSource>(source);
        }

        /// <summary>
        /// Create an <see cref="ReadOnlyCollection{T}"/> from an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static ReadOnlyCollection<TSource> ToReadOnlyCollection<TSource>(this IEnumerable<TSource> source)
        {
            return new ReadOnlyCollection<TSource>(source.ToList());
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        /// <summary>
        /// Orders the by.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns></returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> list, string sortExpression)
        {
            sortExpression += string.Empty;
            string[] parts = sortExpression.Split(' ');
            bool descending = false;

            if (parts.Length > 0 && parts[0] != string.Empty)
            {
                string property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("esc");
                }

                //PropertyInfo prop = typeof(T).GetProperty(property);
                PropertyInfo prop = list.ElementAt(0).GetType().GetProperty(property);

                if (prop == null)
                {
                    throw new Exception("No property '" + property + "' in + " + typeof(T).Name + "'");
                }

                return descending ? list.OrderByDescending(x => prop.GetValue(x, null)) : list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }

        /// <summary>
        /// Produces a sequence of items using a seed value and iteration 
        /// method.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="value">The initial value.</param>
        /// <param name="next">The iteration function.</param>
        /// <returns>A sequence of items using a seed value and iteration 
        /// method.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by at least one consumer of this class.")]
        public static IEnumerable<T> Iterate<T>(T value, Func<T, T> next)
        {
            do
            {
                yield return value;
                value = next(value);
            } while (true);
        }

        /// <summary>
        /// Prepend an item to a sequence.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="that">The sequence to append the item to.</param>
        /// <param name="value">The item to append to the sequence.</param>
        /// <returns>A new sequence.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> that, T value)
        {
            if (that == null)
            {
                throw new ArgumentNullException("that");
            }

            yield return value;
            foreach (T item in that)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Accepts two sequences and applies a function to the corresponding 
        /// values in the two sequences.
        /// </summary>
        /// <typeparam name="T0">The type of the first sequence.</typeparam>
        /// <typeparam name="T1">The type of the second sequence.</typeparam>
        /// <typeparam name="R">The return type of the function.</typeparam>
        /// <param name="enumerable0">The first sequence.</param>
        /// <param name="enumerable1">The second sequence.</param>
        /// <param name="func">The function to apply to the corresponding values
        /// from the two sequences.</param>
        /// <returns>A sequence of transformed values from both sequences.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
        public static IEnumerable<R> Zip<T0, T1, R>(IEnumerable<T0> enumerable0, IEnumerable<T1> enumerable1, Func<T0, T1, R> func)
        {
            IEnumerator<T0> enumerator0 = enumerable0.GetEnumerator();
            IEnumerator<T1> enumerator1 = enumerable1.GetEnumerator();
            while (enumerator0.MoveNext() && enumerator1.MoveNext())
            {
                yield return func(enumerator0.Current, enumerator1.Current);
            }
        }

        /// <summary>
        /// Returns the maximum value in the stream based on the result of a
        /// project function.
        /// </summary>
        /// <typeparam name="T">The stream type.</typeparam>
        /// <param name="that">The stream.</param>
        /// <param name="projectionFunction">The function that transforms the
        /// item.</param>
        /// <returns>The maximum value or null.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used by at least one consumer of this class.")]
        public static T MaxOrNull<T>(this IEnumerable<T> that, Func<T, IComparable> projectionFunction)
            where T : struct
        {
            T maximum = default(T);
            if (!that.Any())
            {
                return maximum;
            }

            maximum = that.First();
            IComparable result = projectionFunction(maximum);
            foreach (T item in that.Skip(1))
            {
                IComparable currentResult = projectionFunction(item);
                if (result.CompareTo(currentResult) >= 0)
                {
                    continue;
                }

                result = currentResult;
                maximum = item;
            }

            return maximum;
        }

        /// <summary>
        /// Returns the maximum value or null if sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="that">The sequence to retrieve the maximum value from.
        /// </param>
        /// <returns>The maximum value or null.</returns>
        public static T? MaxOrNullable<T>(this IEnumerable<T> that)
            where T : struct, IComparable
        {
            if (!that.Any())
            {
                return null;
            }

            return that.Max();
        }

        /// <summary>
        /// Returns the minimum value or null if sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the sequence.</typeparam>
        /// <param name="that">The sequence to retrieve the minimum value from.
        /// </param>
        /// <returns>The minimum value or null.</returns>
        public static T? MinOrNullable<T>(this IEnumerable<T> that)
            where T : struct, IComparable
        {
            if (!that.Any())
            {
                return null;
            }

            return that.Min();
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection,  or returns the default default value for given type.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        public static TSource MinByOrDefault<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinByOrDefault(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values,
        /// or returns the default default value for given type.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        public static TSource MinByOrDefault<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    return default(TSource);
                }
                TSource min = sourceIterator.Current;
                TKey minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    TSource candidate = sourceIterator.Current;
                    TKey candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

       

        // ObservableCollection Extensions

        public static void AddRange<T>(this ObservableCollection<T> oc, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            foreach (T item in collection)
            {
                oc.Add(item);
            }
        }

   

        public static int Remove<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            List<T> removals = collection.Where(predicate).ToList();
            removals.ForEach(r => collection.Remove(r));
            return removals.Count;
        }
    }
}
