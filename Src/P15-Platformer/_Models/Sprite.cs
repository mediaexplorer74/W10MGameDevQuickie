using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Sprite1
    {
        protected Texture2D texture;
        public Vector2 Position { get; protected set; }
        public Vector2 Origin { get; protected set; }
        public Color Color { get; set; }
        public Rectangle Rectangle => new((Position - Origin).ToPoint(), 
            new(texture.Width, texture.Height));

        public Sprite1(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            Position = position;
            Origin = new(texture.Width / 2, texture.Height / 2);
            Color = Color.White;
        }

        public virtual void Draw()
        {
            Glob.SpriteBatch.Draw(texture, Position, null, Color, 0f, 
                Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
