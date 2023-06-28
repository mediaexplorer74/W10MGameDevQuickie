using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{
    public class Circle
    {
        private static readonly Random _r = new();
        private readonly Texture2D _texture;
        public Vector2 Origin { get; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public int Speed { get; set; }
        public Color Color { get; set; }

        private static Vector2 RandomPosition()
        {
            const int padding = 50;
            var x = _r.Next(padding, Glob.Bounds.X - padding);
            var y = _r.Next(padding, Glob.Bounds.Y - padding);
            return new(x, y);
        }

        private static Vector2 RandomDirection()
        {
            var angle = _r.NextDouble() * 2 * Math.PI;
            return new((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }

        public Circle(Texture2D tex)
        {
            _texture = tex;
            Origin = new(tex.Width / 2, tex.Height / 2);
            Speed = 50;//200;
            Position = RandomPosition();
            Direction = RandomDirection();
            Color = Color.White;
        }

        public void Update()
        {
            Position += Direction * Speed * Glob.TotalSeconds;

            if (Position.X < Origin.X || Position.X > Glob.Bounds.X - Origin.X) 
                Direction = new(-Direction.X, Direction.Y);
            if (Position.Y < Origin.Y || Position.Y > Glob.Bounds.Y - Origin.Y) 
                Direction = new(Direction.X, -Direction.Y);

            Position = new(MathHelper.Clamp(Position.X, Origin.X, Glob.Bounds.X - Origin.X),
                           MathHelper.Clamp(Position.Y, Origin.Y, Glob.Bounds.Y - Origin.Y));
        }

        public void Draw()
        {
            Glob.SpriteBatch.Draw(_texture, Position, null, 
                Color, 0, Origin, 1, SpriteEffects.None, 1);
        }
    }
}
