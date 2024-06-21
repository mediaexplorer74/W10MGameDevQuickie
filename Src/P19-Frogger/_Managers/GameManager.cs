using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GameManager
{

    public class GameManager
    {
        private readonly Timer _timer;
        private readonly SpriteFont _font;
        private readonly Vector2 _counterPosition = new Vector2(300, 200);
        private int _counter;

        //private readonly Canvas _canvas;
        private readonly Map1 _map;
        private readonly Hero _hero;
        private readonly List<Monster1> _monsters = new List<Monster1>();
        private readonly Texture2D _monsterTex;
        private readonly Button _button;

        public GameManager(GraphicsDeviceManager graphics)
        {
            //_canvas = new(graphics.GraphicsDevice, 64 * Map1.Size.X, 64 * (Map1.Size.Y + 1));

            /*
            _map = new();
            _hero = new Hero(Glob.Content.Load<Texture2D>("hero"), Vector2.Zero);
            _monsterTex = Glob.Content.Load<Texture2D>("hero");
            Pathfinder.Init(_map, _hero);

            for (int i = 0; i < 10; i++)
            {
                SpawnMonster();
            }

            Monster.OnDeath += (e, a) => SpawnMonster();

            _button = new Button(_monsterTex, new Vector2(32, 13 * 64 - 32));
            _button.OnTap += (e, a) => SpawnMonster();
            */

            _font = Glob.Content.Load<SpriteFont>("TextFont");//("font");

            _timer = new Timer(
               Glob.Content.Load<Texture2D>("timer"),
               _font,
               new Vector2(300, 300),
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

            //if (InputManager.MouseLeftClicked) _timer.StartStop();
            //if (InputManager.MouseRightClicked) _timer.Reset();

            _timer.Update();
        }

        public void Draw()
        {
            //Glob.SpriteBatch.DrawString(_font, _counter.ToString(), _counterPosition, Color.White);
            _timer.Draw();
        }
        /*
        public void SpawnMonster()
        {
            Random r = new();
            Vector2 pos = new(r.Next(64, 448), 0);

            _monsters.Add(new(_monsterTex, pos));
        }

        public void Update()
        {
            //_button.Update();
            //_map.Update();
            //_hero.Update();

            //foreach (var monster in _monsters.ToArray())
            //{
            //    monster.Update();
            //}

            //_monsters.RemoveAll(m => m.Dead);
        }

        public void Draw()
        {
            _canvas.Activate();

            Glob.SpriteBatch.Begin();

            _map.Draw();

            foreach (var monster in _monsters)
            {
                monster.Draw();
            }

            _button.Draw();

            Glob.SpriteBatch.End();

            _canvas.Draw(Glob.SpriteBatch);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _canvas.Activate();

            spriteBatch.Begin();

            _map.Draw();

            foreach (var monster in _monsters)
            {
                monster.Draw();
            }

            _button.Draw();

            spriteBatch.End();

            _canvas.Draw(spriteBatch);
        }
        */
    }
}
