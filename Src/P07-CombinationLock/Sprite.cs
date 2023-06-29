using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CombinationLock
{
    public class Sprite : SpriteBase
    {
        public Texture2D tex;
        public Rectangle drawRect;

        public Sprite(Texture2D loadedTex)
        {
            tex = loadedTex;
            rect = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
            drawRect = new Rectangle((int)position.X, (int)position.Y, 0, 0);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }

        public Sprite(GraphicsDeviceManager graphics)
        {
            tex = new Texture2D(graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            tex.SetData(new[] { color });
            rect = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
            drawRect = new Rectangle((int)position.X, (int)position.Y, 0, 0);
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }

        public override void Update()
        {
            position += velocity;
            rect = new Rectangle((int)position.X, (int)position.Y, tex.Width, tex.Height);
            drawRect.X = (int)position.X;
            drawRect.Y = (int)position.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, position, null, color * alpha, MathHelper.ToRadians(rotation), origin, scale, SpriteEffects.None, 0);
        }

        public void DrawRect(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, drawRect, null, color * alpha, MathHelper.ToRadians(rotation), origin, SpriteEffects.None, 0);
        }
    }
}
