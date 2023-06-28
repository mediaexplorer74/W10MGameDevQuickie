using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;


namespace GameManager
{

    public static class Glob
    {
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static MouseState MouseState { get; set; }
        public static MouseState LastMouseState { get; set; }
        public static bool Clicked { get; set; }
        public static Rectangle MouseCursor { get; set; }

        public static float ElapsedSeconds { get; set; }

        public static void Update(GameTime gameTime)
        {
            LastMouseState = MouseState;
            MouseState = Mouse.GetState();

            Clicked = (MouseState.LeftButton == ButtonState.Pressed) 
                && (LastMouseState.LeftButton == ButtonState.Released);

            MouseCursor = new(MouseState.Position, new(1, 1));

            ElapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

    }
}
