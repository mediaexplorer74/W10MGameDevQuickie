using Microsoft.Xna.Framework;

namespace GameManager.General
{
    /// <summary>
    /// A simple bitmap font class.
    /// </summary>
    class BitmapFont
    {
        private readonly SpriteSheet _sprite;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sprite">Sprite sheet</param>
        public BitmapFont(SpriteSheet sprite)
        {
            _sprite = sprite;
        }

        /// <summary>
        /// Draw a message.
        /// </summary>
        /// <param name="msg">Message</param>
        /// <param name="position">Screen position</param>
        /// <param name="color">Colour</param>
        public void Draw(string msg, Vector2 position, Color color)
        {
            var pos = position;

            foreach(char ch in msg)
            {
                var frame = ch - '!';
                if (frame >= 0)
                {
                    _sprite.Draw(pos, frame, color);
                }

                pos += new Vector2(_sprite.CellWidth, 0);
            }
        }
    }
}
