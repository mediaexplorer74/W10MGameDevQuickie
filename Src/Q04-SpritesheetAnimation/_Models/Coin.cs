using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Coin
    {
        private static Texture2D _texture;
        private Vector2 _position;
        private readonly Animation _anim;

        public Coin(Vector2 pos)
        {
            _texture ??= Glob.Content.Load<Texture2D>("coin");
            _anim = new(_texture, 6, 1, 0.1f);
            _position = pos;
        }

        public void Update()
        {
            _anim.Update();
        }

        public void Draw()
        {
            _anim.Draw(_position);
        }
    }
}
