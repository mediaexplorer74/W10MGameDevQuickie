using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Hero : Sprite
    {
        public bool Moving { get; private set; }
        private Vector2 _destination;
        private Vector2 _ghostPos;
        private const int SPEED = 500;

        public Hero(Texture2D tex) : base(tex, 
            new(Glob.SCREEN_WIDTH / 2, Glob.SCREEN_HEIGHT / 2))
        {
            _destination = position;
        }

        public void Reset()
        {
            position = new(Glob.SCREEN_WIDTH / 2, Glob.SCREEN_HEIGHT / 2);
            _destination = position;
            Moving = false;
        }

        private void CheckDestination()
        {
            _ghostPos = InputManager.MousePosition;

            if (InputManager.MouseLeftClicked)
            {
                _destination = _ghostPos;
                Moving = true;
                Glob.Slowed = false;
            }
        }

        private void UpdateMove()
        {
            if (Moving)
            {
                var dir = _destination - position;

                if (dir.Length() < 5)
                {
                    position = _destination;
                    Moving = false;
                    Glob.Slowed = true;
                    return;
                }

                if (dir != Vector2.Zero)
                {
                    dir.Normalize();
                    position += dir * SPEED * Glob.Time;
                }
            }
        }

        public void Update()
        {
            UpdateMove();
            CheckDestination();
        }

        public override void Draw()
        {
            base.Draw();
            Glob.SpriteBatch.Draw(texture, _ghostPos, 
                null, Color.White * 0.3f, 
                rotation, origin, 1, SpriteEffects.None, 1);
        }
    }
}
