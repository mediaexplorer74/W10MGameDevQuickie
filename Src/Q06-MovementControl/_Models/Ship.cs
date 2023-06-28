using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameManager
{

    public class Ship : Sprite
    {
        private float _rotation;
        private readonly float _rotationSpeed;

        public Ship(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            _rotation = 0;
            _rotationSpeed = 3f;
        }

        public void Update()
        {
            _rotation += InputManager.DirectionArrows.X * _rotationSpeed * Glob.TotalSeconds;
            Vector2 direction = new((float)Math.Sin(_rotation), -(float)Math.Cos(_rotation));
            position += InputManager.DirectionArrows.Y * direction * speed * Glob.TotalSeconds;
        }

        public override void Draw()
        {
            Glob.SpriteBatch.Draw(texture, position, null, 
                Color.White, _rotation, origin, 1, SpriteEffects.None, 1);
        }
    }
}
