using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{

    public class Monster1 : Sprite
    {
        private readonly int _speed;
        private Vector2 _direction;
        public bool Dead { get; private set; }

        public Monster1(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Random r = new Random();
            _speed = r.Next(100, 151);
            _direction = new Vector2(0, 1);
        }

        public static event EventHandler OnDeath;

        public void Die()
        {
            OnDeath?.Invoke(this, EventArgs.Empty);
            Dead = true;
        }

        public void Update()
        {
            if (Dead) return;
            Position += _speed * _direction * Glob.Time;

            if (Position.Y > 768) Die();
        }
    }
}