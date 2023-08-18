using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{

    public class Timer
    {
        private readonly Vector2 _position;
        private readonly SpriteFont _font;
        private string _text;
        private readonly float _timeLength;
        private float _timeLeft;
        private Color _color;

        public Timer(SpriteFont font, Color color, Vector2 position, float length)
        {
            _font = font;
            _color = color;
            _position = position;
            _timeLength = length;
            _timeLeft = length;
        }

        private void FormatText()
        {
            _text = TimeSpan.FromSeconds(_timeLeft).ToString(@"ss\.ff");
        }

        public void Reset()
        {
            _timeLeft = _timeLength;
            FormatText();
        }

        public event EventHandler OnTimer;

        public void Update()
        {
            _timeLeft -= Glob.Time;

            if (_timeLeft <= 0)
            {
                OnTimer?.Invoke(this, EventArgs.Empty);
            }

            FormatText();
        }

        public void Draw()
        {
            Glob.SpriteBatch.DrawString(_font, _text, _position, _color);
        }
    }
}
