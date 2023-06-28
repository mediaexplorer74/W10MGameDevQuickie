using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameManager
{

    public static class Glob
    {

        public static float TotalSeconds { get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        public static void Update(GameTime gt)
        {
            TotalSeconds = (float)gt.ElapsedGameTime.TotalSeconds;
        }



    }
}
