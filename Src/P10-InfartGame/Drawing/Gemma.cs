using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.Drawing
{
    public class Gemma : GameObject
    {
        private float _moveYAmount = 10f;
        private float _elapsed = 0.0f;
        private readonly Texture2D _textureReference;
        private readonly Rectangle _textureRectangle;

        public Gemma(
            Texture2D textureReference,
            Rectangle textureRectangle)
        {
            _textureReference = textureReference;
            _textureRectangle = textureRectangle;

            _collisionRectangle = new Rectangle(
                   0, 0,
                   textureRectangle.Width - 40,
                   textureRectangle.Height - 40);

            Active = false;
        }

        public Gemma(
           Texture2D textureReference,
            Rectangle textureRectangle,
            Vector2 startingPosition)
            : this(textureReference, textureRectangle)
        {
            Position = startingPosition;
        }

        public bool Active { get; set; }

        public override Vector2 Position
        {
            get { return base.Position; }
            set
            {
                _collisionRectangle.X = (int)value.X + 20;
                _collisionRectangle.Y = (int)value.Y + 20;
                base.Position = value;
            }
        }

        public override Rectangle CollisionRectangle
        {
            get { return _collisionRectangle; }
        }

        public int Width
        {
            get { return _textureRectangle.Width; }
        }

        public int Height
        {
            get { return _textureRectangle.Height; }
        }

        public override void Update(double gameTime)
        {
            if (Active)
            {
                float elapsed = (float)gameTime / 1000.0f;

                if (_elapsed >= 0.4f)
                {
                    _moveYAmount *= -1;
                    _elapsed = 0.0f;
                }

                base.Position += new Vector2(0, _moveYAmount * elapsed);

                _elapsed += elapsed;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(
                    _textureReference,
                    base.Position,
                    _textureRectangle,
                    _overlayColor,
                    _rotation,
                    _origin,
                    _scale,
                    _flip,
                    _depth);
            }
        }
    }
}