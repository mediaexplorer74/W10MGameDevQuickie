using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Experience : Sprite
    {
        public float Lifespan { get; private set; }
        private const float LIFE = 5f;

        public Experience(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            Lifespan = LIFE;
        }

        public void Update()
        {
            Lifespan -= Glob.TotalSeconds;
            Scale = 0.33f + (Lifespan / LIFE * 0.66f);
        }

        public void Collect()
        {
            Lifespan = 0;
        }
    }
}
