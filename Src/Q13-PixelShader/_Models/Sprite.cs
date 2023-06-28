using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{
    public class Sprite
    {
        protected readonly Texture2D texture;

        public Sprite(Texture2D tex)
        {
            texture = tex;
        }

        public virtual void Draw(Vector2 pos)
        {
            Glob.SpriteBatch.Draw(texture, pos, Color.White);
        }
    }
}
