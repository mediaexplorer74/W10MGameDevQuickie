using GameManager.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.Background
{
    public class Grattacielo : GameObject
    {
        private readonly int _oneBlockHeight = 25;
        private readonly int _wGrattacielo;
        private readonly Rectangle _textureRectangle;
        private readonly Texture2D _textureReference;

        public Grattacielo(Rectangle textureRectangle, Texture2D textureReference)
        {
            Position = Vector2.Zero;

            _wGrattacielo = textureRectangle.Width;
            HeightInBlocksNumber = textureRectangle.Height / _oneBlockHeight;
            _origin = new Vector2(0, textureRectangle.Height);
            _collisionRectangle.Width = textureRectangle.Width;
            _collisionRectangle.Height = textureRectangle.Height;

            _textureReference = textureReference;
            _textureRectangle = textureRectangle;
        }

        public void Move(Vector2 amount)
        {
            base.Position += amount;
        }

        public override Rectangle CollisionRectangle
        {
            get
            {
                return _collisionRectangle;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                _collisionRectangle.X = (int)value.X;
                _collisionRectangle.Y = (int)(value.Y - _origin.Y);
                base.Position = value;
            }
        }

        public int Width
        {
            get { return _textureRectangle.Width; }
        }

        public int Height
        {
            get { return _textureRectangle.Height; }
        }

        public int HeightInBlocksNumber { get; }

        public Vector2 PositionAtTopLeftCorner()
        {
            return new Vector2(
                Position.X,
                Position.Y - _origin.Y);
        }

        public override void Update(double gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _textureReference,
                base.Position,
                _textureRectangle,
                Color.White,
                0.0f,
                _origin,
                _scale,
                _flip,
                _depth);
        }
    }
}