using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameManager
{
    public static class InputManager
    {
        private static MouseState _lastMouseState;
        public static bool MouseClicked { get; private set; }
        public static bool MouseRightClicked { get; private set; }
        public static Rectangle MouseRectangle { get; private set; }

        private static TouchCollection _lastTouchState;
        public static bool TouchClicked { get; private set; }
        public static bool TouchRightClicked { get; private set; }
        public static Rectangle TouchRectangle { get; private set; }

        public static void Update()
        {
            // 1 Mouse handler
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

            MouseRectangle = new Rectangle(ms.X, ms.Y, 1, 1);

            // 2 TouchPanel handler (TODO)

            TouchCollection ts = TouchPanel.GetState();

            if (ts.Count > 0)
            { 
                var onscreen1 = ts[0].Position.X >= 0
                 && ts[0].Position.X < Glob.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth
                 && ts[0].Position.Y >= 0
                 && ts[0].Position.Y < Glob.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight
                 && Glob.Game.IsActive;

               MouseClicked = (ts[0].State == TouchLocationState.Pressed
                            && _lastTouchState[0].State == TouchLocationState.Released
                            && onscreen);

            // It's not needed for TP but itreseting thing to realize via touch delays! :)
            //MouseRightClicked = (...RightButton == TouchLocationState.Pressed)
            //              && (...RightButton == TTouchLocationState.Released)
            //              && onscreen;
            _lastTouchState = ts;

            MouseRectangle = new Rectangle((int)ts[0].Position.X, (int)ts[0].Position.Y, 1, 1);
           }//if...                      

        }//Update

    }

}
