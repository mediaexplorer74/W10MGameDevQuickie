using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{

    public class GameManager
    {
        private readonly Timer _timer;
        private readonly SpriteFont _font;
        private readonly Vector2 _counterPosition = new(300, 200);
        private int _counter;

        public GameManager()
        {
            _font = Glob.Content.Load<SpriteFont>("font");

            _timer = new(
                Glob.Content.Load<Texture2D>("timer"),
                _font,
                new(300, 300),
                2f
            );

            _timer.OnTimer += IncreaseCounter;
            _timer.StartStop();
            _timer.Repeat = true;
        }

        public void IncreaseCounter(object sender, EventArgs e)
        {
            _counter++;
        }

        public void Update()
        {
            InputManager.Update();

            if (InputManager.MouseLeftClicked) _timer.StartStop();
            if (InputManager.MouseRightClicked) _timer.Reset();

            _timer.Update();
        }

        public void Draw()
        {
            Glob.SpriteBatch.DrawString(_font, _counter.ToString(), _counterPosition, Color.White);
            _timer.Draw();
        }
    }
}
