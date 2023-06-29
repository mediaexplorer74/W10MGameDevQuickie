using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;

namespace FbonizziMonoGame.Drawing
{
    /// <summary>
    /// A no scale scaling matrix
    /// </summary>
    public class IdentityMatrixProvider : IScreenTransformationMatrixProvider
    {
        /// <summary>
        /// No scale scaling matrix provider
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public IdentityMatrixProvider(int width, int height)
        {
            RealScreenWidth = width;
            RealScreenHeight = height;
            VirtualBoundingRectangle = new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Screen width
        /// </summary>
        public int RealScreenWidth { get; }

        /// <summary>
        /// Screen height
        /// </summary>
        public int RealScreenHeight { get; }

        /// <summary>
        /// Virtual width, same as <see cref="RealScreenWidth"/>
        /// </summary>
        public int VirtualWidth => RealScreenWidth;

        /// <summary>
        /// Virtual height, same as <see cref="RealScreenHeight"/>
        /// </summary>
        public int VirtualHeight => RealScreenHeight;

        /// <summary>
        /// Identity matrix
        /// </summary>
        public Matrix ScaleMatrix => Matrix.Identity;

        /// <summary>
        /// Virtual bounding rectangle
        /// </summary>
        public Rectangle VirtualBoundingRectangle { get; }

        /// <summary>
        /// Returns the coordinates itself
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point PointToScreen(int x, int y)
        {
            return new Point(x, y);
        }
    }
}
