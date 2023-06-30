using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        protected Vector2 origin;
        public Texture2D Texture { get; protected set; }
        protected Vector2 scale;
        protected Color color;

        public Sprite(Texture2D tex, Vector2 pos)
        {
            Texture = tex;
            Position = pos;
            origin = new Vector2(tex.Width / 2, tex.Height / 2);
            scale = Vector2.One;
            color = Color.White;
        }

        public virtual void Draw()
        {
            Glob.SpriteBatch.Draw(Texture, Position, null, 
                color, 0f, origin, scale, SpriteEffects.None, 1f);
        }
    }
}
