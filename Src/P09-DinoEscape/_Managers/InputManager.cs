using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameManager
{

    public static class InputManager
    {
        private static Vector2 _direction;
        public static Vector2 Direction => _direction;
        public static bool Moving => _direction != Vector2.Zero;

        public static void Update()
        {
            _direction = Vector2.Zero;
            var keyboardState = Keyboard.GetState();

            //if (keyboardState.GetPressedKeyCount() > 0)
            if (keyboardState.GetPressedKeys().Length > 0)
            {
                if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left)) 
                    _direction.X--;

                if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
                    _direction.X++;

                if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up)) 
                    _direction.Y--;

                if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down)) 
                    _direction.Y++;
            }
        }
    }
}
