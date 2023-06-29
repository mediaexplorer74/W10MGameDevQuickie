using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// SpriteFont extensions
    /// </summary>
    public static class SpriteFontExtensions
    {
        /// <summary>
        /// It gives the text center coordinates when rendered with the given font
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Vector2 GetTextCenter(
            this SpriteFont font,
            StringBuilder text)
        {
            var renderedTextSize = font.MeasureString(text);
            return new Vector2(renderedTextSize.X / 2f, renderedTextSize.Y / 2f);
        }

        /// <summary>
        /// It gives the text center coordinates when rendered with the given font
        /// </summary>
        /// <param name="font"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Vector2 GetTextCenter(
            this SpriteFont font,
            string text)
        {
            var renderedTextSize = font.MeasureString(text);
            return new Vector2(renderedTextSize.X / 2f, renderedTextSize.Y / 2f);
        }

    }
}
