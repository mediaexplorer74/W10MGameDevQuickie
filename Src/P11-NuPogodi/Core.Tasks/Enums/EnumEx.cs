using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Win8.Core.Tasks.Enums
{
    /// <summary>
    /// Class containing helper mehtods related to the enum types.
    /// </summary>
    public class EnumEx
    {
        /// <summary>
        /// Return all possible enum values of selected enum type.
        /// </summary>
        public static IEnumerable<T> GetEnumValues<T>()
            where T : struct, IConvertible
        {
            Type type = typeof(T);
            if (!type.GetTypeInfo().IsEnum)
            {
                throw new InvalidOperationException("GetEnumValues");
            }

            T value = default(T);
            return type.GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => (T)fi.GetValue(value));
        }

        /// <summary>
        /// Convert single int representing multiple flags of given enum, into list of single enum values.
        /// </summary>
        public static List<T> GetFlagsList<T>(int enums)
            where T : struct, IConvertible
        {
            if (!typeof(T).GetTypeInfo().IsEnum)
            {
                throw new Exception("Type given must be an Enum");
            }

            return GetEnumValues<T>().Cast<int>()
                .Where(i => (enums & i) == i).Select(i => (T)Enum.Parse(typeof(T), i.ToString(), true)).ToList();
        }
    }
}
