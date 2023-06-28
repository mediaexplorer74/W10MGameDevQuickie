using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class GameManager
    {
        private readonly Sprite _sprite;
        private readonly Vector2 _pos00, _pos01, _pos02, _pos03, _pos04;
        private readonly Effect _effect01, _effect02, _effect03, _effect04;
        private float _amount = 1;
        private float _dir = -1;

        public GameManager()
        {
            _sprite = new(Glob.Content.Load<Texture2D>("orb-red"));
            _pos00 = new(100, 100);
            _pos01 = new(200, 100);
            _pos02 = new(300, 100);
            _pos03 = new(400, 100);
            _pos04 = new(500, 100);
            _effect01 = Glob.Content.Load<Effect>("effect01.fx");
            _effect02 = Glob.Content.Load<Effect>("effect02.fx");
            _effect03 = Glob.Content.Load<Effect>("effect03.fx");
            _effect04 = Glob.Content.Load<Effect>("effect04.fx");
        }

        public void Update()
        {
            _amount += Glob.TotalSeconds * _dir;
            if (_amount < 0 || _amount > 1) _dir *= -1;
            _effect04.Parameters["amount"].SetValue(_amount);
        }

        public void Draw()
        {
            Glob.SpriteBatch.Begin();
            _sprite.Draw(_pos00);
            Glob.SpriteBatch.End();
            Glob.SpriteBatch.Begin(effect: _effect01);
            _sprite.Draw(_pos01);
            Glob.SpriteBatch.End();
            Glob.SpriteBatch.Begin(effect: _effect02);
            _sprite.Draw(_pos02);
            Glob.SpriteBatch.End();
            Glob.SpriteBatch.Begin(effect: _effect03);
            _sprite.Draw(_pos03);
            Glob.SpriteBatch.End();
            Glob.SpriteBatch.Begin(effect: _effect04);
            _sprite.Draw(_pos04);
            Glob.SpriteBatch.End();
        }
    }
}
