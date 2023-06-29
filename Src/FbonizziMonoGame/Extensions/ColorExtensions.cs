using Microsoft.Xna.Framework;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// Colore class extensions
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// Fluent syntax to get Color * alpha
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        /// <returns></returns>
        public static Color WithAlpha(this Color color, float alpha)
            => color * alpha;

        /// <summary>
        /// Linear interpolation between two colors
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Color Lerp(this Color value1, Color value2, float amount)
        {
            amount = MathHelper.Clamp(amount, 0, 1);
            return new Color()
            {
                R = (byte)MathHelper.Lerp(value1.R, value2.R, amount),
                G = (byte)MathHelper.Lerp(value1.G, value2.G, amount),
                B = (byte)MathHelper.Lerp(value1.B, value2.B, amount),
                A = (byte)MathHelper.Lerp(value1.A, value2.A, amount)
            };
        }
    }
}
