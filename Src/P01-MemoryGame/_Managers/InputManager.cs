using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameManager
{
    public static class InputManager
    {
        private static MouseState _lastMouseState;
        public static bool MouseClicked { get; private set; }
        public static bool MouseRightClicked { get; private set; }
        public static Rectangle MouseRectangle { get; private set; }

        public static void Update()
        {
            var ms = Mouse.GetState();
            var onscreen = ms.X >= 0 
             && ms.X < Glob.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth
             && ms.Y >= 0 
             && ms.Y < Glob.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight
             && Glob.Game.IsActive;

            MouseClicked = (ms.LeftButton == ButtonState.Pressed)
                            && (_lastMouseState.LeftButton == ButtonState.Released)
                            && onscreen;
            MouseRightClicked = (ms.RightButton == ButtonState.Pressed)
                            && (_lastMouseState.RightButton == ButtonState.Released)
                            && onscreen;
            _lastMouseState = ms;

            MouseRectangle = new(ms.X, ms.Y, 1, 1);
        }
    }
}
