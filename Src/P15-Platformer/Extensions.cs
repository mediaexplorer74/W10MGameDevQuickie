using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager
{
    static class Extensions
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Texture2D tex, Rectangle pixel, Vector2 begin, Vector2 end, Color color, int thick = 1)
        {
            Vector2 delta = end - begin;
            float rot = (float)Math.Atan2(delta.Y, delta.X);
            if (pixel.Width > 0) { pixel.Width = 1; pixel.Height = 1; }
            spriteBatch.Draw(tex, begin, pixel, color, rot, new Vector2(0, 0.5f), new Vector2(delta.Length(), thick), SpriteEffects.None, 0);
        }


        public static void DrawRectLines(this SpriteBatch spriteBatch, Texture2D tex, Rectangle pixel, Rectangle r, Color color, int thick = 1) {
            spriteBatch.Draw(tex, new Rectangle(r.X, r.Y, r.Width, thick), pixel, color);
            spriteBatch.Draw(tex, new Rectangle(r.X + r.Width, r.Y, thick, r.Height), pixel, color);
            spriteBatch.Draw(tex, new Rectangle(r.X, r.Y + r.Height, r.Width, thick), pixel, color);
            spriteBatch.Draw(tex, new Rectangle(r.X, r.Y, thick, r.Height), pixel, color);        
        }
    }
}
