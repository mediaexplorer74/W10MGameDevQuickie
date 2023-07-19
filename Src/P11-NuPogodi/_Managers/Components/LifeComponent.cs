using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.AppLogic.Components
{
    public class LifeComponent : ItemComponent
    {
        protected static Texture2D Texture;

        public LifeComponent(ContentManager contentManager)
            : base(contentManager)
        {
        }

        public override void LoadContent()
        {
            if (Texture == null)
            {
                Texture = ContentManager.Load<Texture2D>("images/life");
            }
        }

        public override void UnloadContent()
        {
            Texture = null;
        }

        public override void Draw(GameTime gameTime)
        {
            int life = GameController.I.EggDrops;
            int eggs = (life + 1)/2;

            for (int i = 0; i < eggs; i++)
            {
                // do not draw blinking life
                if (i == eggs-1 && life%2 == 1 && gameTime.TotalGameTime.Milliseconds < 500) return;

                SpriteBatch.Draw(Texture, new Rectangle(479 - 30 * i, 194, Texture.Width, Texture.Height), Color.White);
            }
        }
    }
}
