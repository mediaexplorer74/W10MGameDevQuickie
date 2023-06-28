using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class UI
    {
        private readonly SpriteFont _font;
        private int _gems;
        private readonly Vector2 _pos;

        public UI(SpriteFont font, Vector2 pos, Hero hero)
        {
            _font = font;
            _pos = pos;
            hero.OnCollect += ObserveGems;
        }

        private void ObserveGems(int gems)
        {
            _gems = gems;
        }

        public void Draw()
        {
            Glob.SpriteBatch.DrawString(_font, _gems.ToString(), _pos, Color.White);
        }
    }
}
