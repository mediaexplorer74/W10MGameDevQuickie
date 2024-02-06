using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager.General
{
    /// <summary>
    /// A basic sprite sheet.
    /// </summary>
    class SpriteSheet
    {
        private readonly Texture2D _texture;
        private readonly SpriteBatch _spriteBatch;
        private readonly int _columns;
        private readonly int _rows;

        /// <summary>
        /// Get the width of a cell.
        /// </summary>
        public int CellWidth { get; }

        /// <summary>
        /// Get the height of a cell.
        /// </summary>
        public int CellHeight { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="texture">Texture</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="cellWidth">Cell width</param>
        /// <param name="cellHeight">Cell height</param>
        public SpriteSheet(Texture2D texture, SpriteBatch spriteBatch, int cellWidth, int cellHeight)
        {
            _texture = texture;
            _spriteBatch = spriteBatch;
            CellWidth = cellWidth;
            CellHeight = cellHeight;

            _columns = _texture.Width / CellWidth;
            _rows = _texture.Height / CellHeight;
        }

        /// <summary>
        /// Draw a sprite on screen.
        /// </summary>
        /// <param name="position">Screen position</param>
        /// <param name="frame">Frame</param>
        /// <param name="color">Colour</param>
        public void Draw(Vector2 position, int frame, Color color)
        {
            Draw(position, frame, color, SpriteEffects.None);
        }

        /// <summary>
        /// Draw a sprite on screen.
        /// </summary>
        /// <param name="position">Screen position</param>
        /// <param name="frame">Frame</param>
        /// <param name="color">Colour</param>
        /// <param name="effect">Sprite effect</param>
        public void Draw(Vector2 position, int frame, Color color, SpriteEffects effect)
        {
            if (frame < 0 || frame >= _columns * _rows)
                throw new ArgumentOutOfRangeException($"{frame} is out of range!");

            var column = frame % _columns;
            var row = frame / _columns;
            var x = column * CellWidth;
            var y = row * CellHeight;

            _spriteBatch.Draw(_texture,
                             position,
                             new Rectangle(x, y, CellWidth, CellHeight),
                             color,
                             0f,
                             Vector2.Zero,
                             new Vector2(1, 1), 
                             effect,
                             0f);
        }
    }
}
