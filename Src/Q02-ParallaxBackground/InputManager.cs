using Microsoft.Xna.Framework.Input;

namespace GameManager
{

    public class InputManager
    {
        private readonly float _speed = 1200f;
        public float Movement { get; set; }

        public void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            Movement = 0;

            if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Left))
            {
                Movement = -_speed;
            }
            else if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Right))
            {
                Movement = _speed;
            }
        }
    }
}
