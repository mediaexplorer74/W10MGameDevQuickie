using System;
using System.Linq;

namespace Win8.Core.Tasks.Enums
{
    /// <summary>
    /// Extension methods related to the DateTime structure.
    /// </summary>
    public class DateTimeEx
    {
        /// <summary>
        /// List of days in a week
        /// </summary>
        public static DayOfWeek[] DaysOfWeek
        {
            get { return daysOfWeek ?? (daysOfWeek = EnumEx.GetEnumValues<DayOfWeek>().ToArray()); }
        }
        private static DayOfWeek[] daysOfWeek;

        /// <summary>
        /// Finds the next date whose day of the week equals the specified day of the week.
        /// </summary>
        public static DateTime GetNextDateForDay(DateTime date, DayOfWeek desiredDay)
        {
            // Given a date and day of week,
            // find the next date whose day of the week equals the specified day of the week.
            return date.AddDays(DaysToAdd(date.DayOfWeek, desiredDay));
        }

        /// <summary>
        /// Calculates the number of days to add to the given day of
        /// the week in order to return the next occurrence of the
        /// desired day of the week.
        /// </summary>
        public static int DaysToAdd(DayOfWeek current, DayOfWeek desired)
        {
            // f( c, d ) = g( c, d ) mod 7, g( c, d ) > 7
            //           = g( c, d ), g( c, d ) < = 7
            //   where 0 <= c < 7 and 0 <= d < 7
            int c = (int)current;
            int d = (int)desired;
            int n = (7 - c + d);

            return (n > 7) ? n % 7 : n;
        }
    }
}
