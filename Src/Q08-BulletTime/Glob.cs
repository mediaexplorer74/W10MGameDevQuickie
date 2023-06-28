using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameManager
{

    public static class Glob
    {

        public const int SCREEN_WIDTH = 640;//1024;
        public const int SCREEN_HEIGHT = 480;//768;
        public static float Time => TimeManager.Time;
        public static float BulletTime => TimeManager.BulletTime;
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        public static void Update(GameTime gt)
        {
            TimeManager.Update(gt);
        }


    }
}
