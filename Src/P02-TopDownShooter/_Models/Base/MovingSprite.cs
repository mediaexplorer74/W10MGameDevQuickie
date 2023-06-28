using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class MovingSprite : Sprite
    {
        public int Speed { get; set; }

        public MovingSprite(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            Speed = 300;
        }
    }
}
