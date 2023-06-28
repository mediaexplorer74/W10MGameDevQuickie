using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameManager
{
    public static class Glob
    {
        public static int WindowWidth = 640;
        public static int WindowHeight = 480;

        public static float Time { get; private set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        public static void Update(GameTime gt)
        {
            Time = (float)gt.ElapsedGameTime.TotalSeconds;
        }


    }
}
