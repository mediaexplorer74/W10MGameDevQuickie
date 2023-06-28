using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Sprite
    {
        protected Texture2D texture;
        public Vector2 Position { get; protected set; }
        public Rectangle Rectangle
        {
            get
            {
                return new((int)Position.X, (int)Position.Y,
                              texture.Width, texture.Height);
            }
        }

        public Sprite(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            Position = position;
        }

        public virtual void Draw()
        {
            Glob.SpriteBatch.Draw(texture, Position, null, Color.White,
                0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
