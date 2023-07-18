using System;

namespace Win8.Core.Tasks.Extensions
{
    /// <summary>
    /// Extensions related to string operations.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Return relative <see cref="Uri"/> base on the input string.
        /// </summary>
        public static Uri ToRelUri(this string str)
        {
            return new Uri(str, UriKind.Relative);
        }

        /// <summary>
        /// Return absolute <see cref="Uri"/> base on the input string.
        /// </summary>
        public static Uri ToAbsUri(this string str)
        {
            return new Uri(str, UriKind.Absolute);
        }
    }
}