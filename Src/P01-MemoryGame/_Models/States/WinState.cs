using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{
    public class WinState : GameState
    {
        private static Texture2D _texture;
        private static Vector2 _position;
        private static SpriteFont _font;
        private static Vector2 _textPosition;
        private static string _text = "";

        public WinState()
        {
            _texture = Glob.Content.Load<Texture2D>("Menu/win");
            _font = Glob.Content.Load<SpriteFont>("Menu/font");

            _position = new((Glob.Bounds.X - _texture.Width) / 2, 
                (Glob.Bounds.Y - _texture.Height) / 2);
        }

        public override void Update(GameManager gm)
        {
            if (InputManager.MouseClicked)
            {
                gm.ChangeState(GameStates.Menu);
            }

            _text = ScoreManager.Score.ToString();
            var size = _font.MeasureString(_text);
            _textPosition = new((Glob.Bounds.X - size.X) / 2,
                _position.Y + (_texture.Height / 4));
        }

        public override void Draw(GameManager gm)
        {
            Glob.SpriteBatch.Draw(_texture, _position, Color.White);
            Glob.SpriteBatch.DrawString(_font, _text, _textPosition, Color.Black);
        }
    }
}
