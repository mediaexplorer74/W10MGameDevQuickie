using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{
    public class RotatingSprite : Sprite
    {
        private float _rotation;
        private Vector2 _direction;

        public RotatingSprite(Texture2D tex, Vector2 pos, Vector2 dir) : base(tex, pos)
        {
            var r = new Random();
            _direction = dir;
            _rotation = /*r.NextSingle()*/ (float)r.NextDouble() * 2 * (float)Math.PI;
        }

        public void Update()
        {
            _rotation += Glob.Time;
            Position += _direction * Glob.Time * 75;

            if (Position.X + origin.X < 0) 
                Position = new Vector2(Glob.Bounds.X + origin.X, Position.Y);

            if (Position.X - origin.X > Glob.Bounds.X) 
                Position = new Vector2(-origin.X, Position.Y);

            if (Position.Y + origin.Y < 0) 
                Position = new Vector2(Position.X, Glob.Bounds.Y + origin.Y);

            if (Position.Y - origin.Y > Glob.Bounds.Y) 
                Position = new Vector2(Position.X, -origin.Y);
        }

        public override void Draw()
        {
            Glob.SpriteBatch.Draw(Texture, Position, null, 
                Color.White * 0.2f, _rotation, origin, Vector2.One, SpriteEffects.None, 1f);
        }
    }
}
