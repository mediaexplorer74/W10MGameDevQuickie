using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class OrbKeyboard : Sprite
    {
        public OrbKeyboard(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
        }

        public void Update()
        {
            if (InputManager.Direction != Vector2.Zero)
            {
                var dir = Vector2.Normalize(InputManager.Direction);
                position += dir * speed * Glob.TotalSeconds;
            }
        }
    }
}