using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FbonizziMonoGame.Extensions
{
    /// <summary>
    /// Texture2D extensions
    /// </summary>
    public static class Texture2DExtensions
    {
        /// <summary>
        /// It crops a Texture2D with the specified crop rectangle
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="graphics"></param>
        /// <param name="cropRectangle"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(
            this Texture2D texture, 
            GraphicsDevice graphics, 
            Rectangle cropRectangle)
        {
            Texture2D tex = new Texture2D(graphics, cropRectangle.Width, cropRectangle.Height);
            int count = cropRectangle.Width * cropRectangle.Height;
            Color[] data = new Color[count];
            texture.GetData(0, cropRectangle, data, 0, count);
            tex.SetData(data);
            return tex;
        }
    }
}
