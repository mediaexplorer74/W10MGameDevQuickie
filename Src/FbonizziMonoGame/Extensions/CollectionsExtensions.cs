using System.Collections.Generic;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// A set of extensions to collections
    /// </summary>
    public static class CollectionsExtensions
    {
        /// <summary>
        /// It shuffles a collection
        /// Implementa Fisher-Yeats shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Numbers.RandomBetween(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
