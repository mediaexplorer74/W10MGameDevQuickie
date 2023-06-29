using Microsoft.Xna.Framework;

namespace FbonizziMonoGame.Drawing
{
    /// <summary>
    /// It represents basic informations about how and where to draw a drawable object
    /// </summary>
    public class DrawingInfos
    {
        /// <summary>
        /// The object position
        /// </summary>
        public Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>
        /// True if the object image is flipped
        /// </summary>
        public bool IsFlipped { get; set; } = false;

        /// <summary>
        /// The overlay color of the object
        /// </summary>
        public Color OverlayColor { get; set; } = Color.White;

        /// <summary>
        /// The origin of the spatial axes of the cartesian representation of the object
        /// </summary>
        public Vector2 Origin { get; set; } = Vector2.Zero;

        /// <summary>
        /// The object scale
        /// </summary>
        public float Scale { get; set; } = 1.0f;

        /// <summary>
        /// The object rotation factor
        /// </summary>
        public float Rotation { get; set; } = 0.0f;

        /// <summary>
        /// The object hit box tolerance
        /// </summary>
        public Rectangle HitBoxTolerance { get; set; } = Rectangle.Empty;

        /// <summary>
        /// The object layer depth for drawing sort
        /// </summary>
        public float LayerDepth { get; set; } = 0.0f;

        /// <summary>
        /// The object hitbox calculated with <see cref="HitBoxTolerance"/>, given its dimensions
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Rectangle HitBox(int width, int height)
        {
            return new Rectangle(
                (int)(Position.X - Origin.X) + HitBoxTolerance.X,
                (int)(Position.Y - Origin.Y) + HitBoxTolerance.Y,
                (width - HitBoxTolerance.Width),
                (height - HitBoxTolerance.Height));
        }

        /// <summary>
        /// The object cartesian representation center
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public Vector2 Center(int width, int height)
        {
            var originCalculatedPosition = Position - Origin;
            return new Vector2(
                originCalculatedPosition.X + width / 2,
                originCalculatedPosition.Y + height / 2);
        }
    }
}