using Microsoft.Xna.Framework;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// Rectangle manipulation extensions
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Calculates a rectangle within a tolerance, like a "padding"
        /// </summary>
        /// <param name="me"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static Rectangle WithTolerance(
            this Rectangle me,
            Rectangle tolerance)
        {
            return new Rectangle(
                me.X + tolerance.X,
                me.Y + tolerance.Y,
                me.Width - tolerance.Width,
                me.Height - tolerance.Height);
        }

        /// <summary>
        /// Calculates a rectangle within a tolerance, like a "padding"
        /// </summary>
        /// <param name="me"></param>
        /// <param name="commonTolerance"></param>
        /// <returns></returns>
        public static Rectangle WithTolerance(
            this Rectangle me,
            int commonTolerance)
        {
            var tolerance = new Rectangle(commonTolerance, commonTolerance, commonTolerance, commonTolerance);

            return me.WithTolerance(tolerance);
        }
    }
}
