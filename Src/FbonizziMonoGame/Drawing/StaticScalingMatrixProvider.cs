using FbonizziMonoGame.Drawing.Abstractions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FbonizziMonoGame.Drawing
{
    /// <summary>
    /// Generates a scaling view matrix calculated only once created
    /// </summary>
    public class StaticScalingMatrixProvider : IScreenTransformationMatrixProvider
    {
        /// <summary>
        /// The device screen width in which the <see cref="VirtualWidth"/> has to fit 
        /// </summary>
        public int RealScreenWidth { get; }

        /// <summary>
        /// The device screen height in which the <see cref="VirtualHeight"/> has to fit 
        /// </summary>
        public int RealScreenHeight { get; }

        /// <summary>
        /// The virtual width that should fit to the <see cref="RealScreenWidth"/>
        /// </summary>
        public int VirtualWidth { get; }

        /// <summary>
        /// The virtual height that should fit to the <see cref="RealScreenHeight"/>
        /// </summary>
        public int VirtualHeight { get; }

        /// <summary>
        /// The scale matrix that defines the transformation needed to fit <see cref="VirtualWidth"/> and <see cref="VirtualHeight"/>
        /// into <see cref="RealScreenWidth"/> and <see cref="RealScreenHeight"/>
        /// </summary>
        public Matrix ScaleMatrix { get; }

        /// <summary>
        /// Virtual bounding rectangle
        /// </summary>
        public Rectangle VirtualBoundingRectangle => new Rectangle(0, 0, VirtualWidth, VirtualHeight);

        /// <summary>
        /// Projects the given coordinates into screen coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point PointToScreen(int x, int y)
        {
            var invertedMatrix = Matrix.Invert(ScaleMatrix);
            return Vector2.Transform(new Vector2(x, y), invertedMatrix).ToPoint();
        }

        /// <summary>
        /// Static scaling matrix provider constructor
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="virtualWidth"></param>
        /// <param name="virtualHeight"></param>
        /// <param name="mantainProportions"></param>
        public StaticScalingMatrixProvider(
            GraphicsDevice graphicsDevice,
            int virtualWidth,
            int virtualHeight,
            bool mantainProportions)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException(nameof(graphicsDevice));
            }

            RealScreenWidth = graphicsDevice.Viewport.Width;
            RealScreenHeight = graphicsDevice.Viewport.Height;

            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;

            var scaleX = (float)RealScreenWidth / virtualWidth;
            var scaleY = (float)RealScreenHeight / virtualHeight;

            if (!mantainProportions)
            {
                ScaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
            }
            else
            {
                var screenCenter = new Vector2(RealScreenWidth / 2f, RealScreenHeight / 2f);
                var origin = new Vector2(VirtualWidth / 2f, VirtualHeight / 2f);
                var scale = Math.Min(scaleX, scaleY);
                ScaleMatrix =
                       Matrix.CreateTranslation(new Vector3(-origin, 0.0f))
                      * Matrix.CreateScale(scale, scale, 1.0f)
                      * Matrix.CreateTranslation(new Vector3(screenCenter, 0f));
            }
        }

    }
}
