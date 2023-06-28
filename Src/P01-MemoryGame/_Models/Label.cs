using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{
    public class Label
    {
        private readonly SpriteFont _font;
        private Vector2 _centerPos;
        private Vector2 _pos;
        public string Text { get; private set; }

        public Label(SpriteFont font, Vector2 position)
        {
            _font = font;
            _centerPos = position;
        }

        public void SetText(string text)
        {
            Text = text;
            _pos = new(_centerPos.X - (_font.MeasureString(Text).X / 2) + 3, _centerPos.Y);
        }

        public void Draw()
        {
            Glob.SpriteBatch.DrawString(_font, Text, _pos, Color.Black);
        }
    }
}
