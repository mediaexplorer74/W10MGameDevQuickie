using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Hero : Sprite
    {
        protected float speed;
        private readonly Trail _trail;

        public Hero(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            speed = 400;
            _trail = new(Glob.Content.Load<Texture2D>("feet"), Position);
        }

        public void Update()
        {
            var change = InputManager.Direction * Glob.Time * speed;
            Position += change;
            _trail.Update(Position);
        }

        public override void Draw()
        {
            _trail.Draw();
            base.Draw();
        }
    }
}