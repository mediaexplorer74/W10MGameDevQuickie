using System;

namespace FbonizziMonoGame
{
    /// <summary>
    /// Numbers manipulation methods
    /// </summary>
    public static class Numbers
    {
        private static readonly Random Rand = new Random();

        /// <summary>
        /// It maps a value from the interval [A; B] to the interval [newA; newB]
        /// </summary>
        /// <param name="value">The value to map</param>
        /// <param name="A">Source interval first member</param>
        /// <param name="B">Source interval second member</param>
        /// <param name="newA">Destination interval first member</param>
        /// <param name="newB">Destination interval second member</param>
        /// <returns></returns>
        public static float MapValueFromIntervalToInterval(
            float value, 
            float A, float B, 
            float newA, float newB)
        {
            // Division by zero!
            // (B - A) == 0

            return newA + ((newB - newA) / (B - A)) * (value - A);
        }

        /// <summary>
        /// It maps a value from the interval [A; B] to an interval defined by its first member and a slope
        /// </summary>
        /// <param name="value">The value to map</param>
        /// <param name="A">Source interval first member</param>
        /// <param name="newA">Destination interval first member</param>
        /// <param name="slope">Slope: (newB - newA) / (B - A)</param>
        /// <returns></returns>
        public static float MapValueFromIntervalToInterval(float value, float A, float newA, float slope)
            => newA + slope * (value - A);
               
        /// <summary>
        /// Random float number
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float RandomBetween(float min, float max)
            => min + (float)Rand.NextDouble() * (max - min);
        
        /// <summary>
        /// Random double number
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double RandomBetween(double min, double max)
            => min + Rand.NextDouble() * (max - min);

        /// <summary>
        /// Random integer number
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomBetween(int min, int max)
            => Rand.Next(min, max);

        /// <summary>
        /// Random time interval
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static TimeSpan RandomBetween(TimeSpan min, TimeSpan max)
            => TimeSpan.FromSeconds(RandomBetween(min.TotalSeconds, max.TotalSeconds));

        /// <summary>
        /// It calculates a value with the Sin function and maps it into a given interval
        /// </summary>
        /// <param name="overTimeValue"></param>
        /// <param name="minDelta"></param>
        /// <param name="maxDelta"></param>
        /// <returns></returns>
        public static float GenerateDeltaOverTimeSin(
            float overTimeValue, float minDelta, float maxDelta)
            => MapValueFromIntervalToInterval(
                (float)Math.Sin(overTimeValue),
                -1.0f, +1.0f,
                minDelta, maxDelta);

        /// <summary>
        /// It calculates a value with the Cos function and maps it into a given interval
        /// </summary>
        /// <param name="overTimeValue"></param>
        /// <param name="minDelta"></param>
        /// <param name="maxDelta"></param>
        /// <returns></returns>
        public static float GenerateDeltaOverTimeCos(
            float overTimeValue, float minDelta, float maxDelta)
            => MapValueFromIntervalToInterval(
                (float)Math.Cos(overTimeValue),
                -1.0f, +1.0f,
                minDelta, maxDelta);
    }
}
