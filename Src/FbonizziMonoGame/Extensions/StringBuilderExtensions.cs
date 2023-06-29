using System.Text;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// StringBuilder extensions
    /// </summary>
    public static class StringBuilderExtensions
    {
        // 11 characters will fit -4294967296
        static char[] numberBuffer = new char[11];

        /// <summary>
        /// Append an integer without generating any garbage.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static StringBuilder AppendNumber(this StringBuilder sb, int number)
        {
            bool negative = (number < 0);
            if (negative)
                number = -number;

            int i = numberBuffer.Length;
            do
            {
                numberBuffer[--i] = (char)('0' + (number % 10));
                number /= 10;
            }
            while (number > 0);

            if (negative)
                numberBuffer[--i] = '-';

            sb.Append(numberBuffer, i, numberBuffer.Length - i);

            return sb;
        }
    }
}
