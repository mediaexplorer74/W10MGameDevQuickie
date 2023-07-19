using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading;

namespace GameManager
{

    public class GameManager
    {
        private readonly Timer _timer;
        private readonly SpriteFont _font;
        private readonly Vector2 _counterPosition = new Vector2(10, 10);//(300, 200);
        private /*int*/ double _counter;

        public GameManager()
        {
            //_font = Glob.Content.Load<SpriteFont>("TextFont");//("font");

            _timer = new Timer(
                Glob.Content.Load<Texture2D>("timer"),
                _font,
                new Vector2(10,10),//(300, 300),
                20f//2f
            );

            _timer.OnTimer += IncreaseCounter;
            _timer.StartStop();
            _timer.Repeat = true;
        }

        public void IncreaseCounter(object sender, EventArgs e)
        {
            _counter = _counter + 0.1;//++;
        }

        public void Update()
        {
            InputManager.Update();

            //if (InputManager.MouseLeftClicked) _timer.StartStop();
            if (InputManager.MouseRightClicked) _timer.StartStop();//_timer.Reset();

            _timer.Update();
        }

        public void Draw()
        {
            //Glob.SpriteBatch.DrawString(_font, _counter.ToString(), _counterPosition, Color.White);
            _timer.Draw();
        }
    }
}
