using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameManager
{

    public class GameManager
    {
        private const int BULLET_COUNT = 100;
        private readonly List<Bullet> _bullets = new();

        public GameManager()
        {
            var bulletTexture = Glob.Content.Load<Texture2D>("bullet");

            for (int i = 0; i < BULLET_COUNT; i++)
            {
                _bullets.Add(new(bulletTexture));
            }
        }

        public void Update()
        {
            foreach (var _bullet in _bullets)
            {
                _bullet.Update();
            }
        }

        public void Draw()
        {
            foreach (var _bullet in _bullets)
            {
                _bullet.Draw();
            }
        }
    }
}
