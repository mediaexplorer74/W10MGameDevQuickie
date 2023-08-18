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
        public const int SCREEN_WIDTH = 1600;
        public const int SCREEN_HEIGHT = 900;
        public static float Time { get; private set; }
        public static float ElapsedTime { get; private set; }
        public static bool Slowed { get; set; } = true;
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        public static void Update(GameTime gt)
        {
            ElapsedTime = (float)gt.ElapsedGameTime.TotalSeconds;
            Time = ElapsedTime * ((Slowed && !InputManager.MouseRightDown) ? 0.02f : 1f);
        }

    }
}
