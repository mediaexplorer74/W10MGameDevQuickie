using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.AppLogic.Components
{
    public class ZajicComponent : ItemComponent
    {
        protected static Texture2D Texture;

        public ZajicComponent(ContentManager contentManager)
            : base(contentManager)
        {
        }

        public override void LoadContent()
        {
            if (Texture == null)
            {
                Texture = ContentManager.Load<Texture2D>("images/zajic");
            }
        }

        public override void UnloadContent()
        {
            Texture = null;
        }

        public override void Draw(GameTime gameTime)
        {
            if (GameController.I.Zajic)
            {
                SpriteBatch.Draw(Texture, new Rectangle(240, 126, Texture.Width, Texture.Height), Color.White);
            }
        }
    }
}
