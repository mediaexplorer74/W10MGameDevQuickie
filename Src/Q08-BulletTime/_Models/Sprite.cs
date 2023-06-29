using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Sprite
    {
        protected readonly Texture2D texture;
        protected readonly Vector2 origin;
        protected Vector2 position;
        protected float rotation;

        public Sprite(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            position = pos;
            origin = new(tex.Width / 2, tex.Height / 2);
        }

        public virtual void Draw()
        {
            Glob.SpriteBatch.Draw(texture, position, null, 
                Color.White, rotation, origin, 1, SpriteEffects.None, 1);
        }
    }
}