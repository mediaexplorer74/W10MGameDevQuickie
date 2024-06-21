using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameManager
{

    public class GameManager
    {
        private readonly Canvas _canvas;
        private readonly Map1 _map;
        private readonly Hero _hero;
        private readonly List<Monster1> _monsters = new List<Monster1>();
        private readonly Texture2D _monsterTex;
        private readonly Button _button;

        public GameManager(GraphicsDeviceManager graphics)
        {
            _canvas = new Canvas(graphics.GraphicsDevice, 64 * Map1.Size.X, 64 * (Map1.Size.Y + 1));
            
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
        }

        public void SpawnMonster()
        {
            Random r = new Random();
            Vector2 pos = new Vector2(r.Next(64, 448), 0);

            _monsters.Add(new Monster1(_monsterTex, pos));
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
    }
}
