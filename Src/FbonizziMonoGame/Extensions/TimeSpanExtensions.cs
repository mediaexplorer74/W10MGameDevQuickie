using System;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// TimeSpan extensions
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// It returns a string starting from a TimeSpan in the format mm:ss
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static string ToMinuteSecondsFormat(this TimeSpan timeSpan)
            => timeSpan.ToString(@"mm\:ss");
    }
}
