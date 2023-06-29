using Microsoft.Xna.Framework;

namespace FbonizziMonoGame.Drawing.Abstractions
{
    /// <summary>
    /// Generates a view matrix that represents the transformation needed
    /// to scale a virtual rectangle to fit the real graphics device viewport.
    /// For example: your game view screen is a 800x600 rectangle but you need to fit 
    /// it into a 1920x1080 device screen.
    /// </summary>
    public interface IScreenTransformationMatrixProvider
    {
        /// <summary>
        /// The device screen width in which the <see cref="VirtualWidth"/> has to fit 
        /// </summary>
        int RealScreenWidth { get; }

        /// <summary>
        /// The device screen height in which the <see cref="VirtualHeight"/> has to fit 
        /// </summary>
        int RealScreenHeight { get; }

        /// <summary>
        /// The virtual width that should fit to the <see cref="RealScreenWidth"/>
        /// </summary>
        int VirtualWidth { get; }

        /// <summary>
        /// The virtual height that should fit to the <see cref="RealScreenHeight"/>
        /// </summary>
        int VirtualHeight { get; }

        /// <summary>
        /// The scale matrix that defines the transformation needed to fit <see cref="VirtualWidth"/> and <see cref="VirtualHeight"/>
        /// into <see cref="RealScreenWidth"/> and <see cref="RealScreenHeight"/>
        /// </summary>
        Matrix ScaleMatrix { get; }

        /// <summary>
        /// Projects the given coordinates into screen coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Point PointToScreen(int x, int y);

        /// <summary>
        /// Virtual bounding rectangle
        /// </summary>
        Rectangle VirtualBoundingRectangle { get; }
    }
}
