using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.AppLogic.Components
{
    public abstract class ItemComponent : GameComponent
    {
        protected readonly ContentManager ContentManager;
        public SpriteBatch SpriteBatch;

        protected ItemComponent(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }

        public override void Initialize()
        {
            // intentionally left blank
        }

        public override void Update(GameTime gameTime)
        {
            // intentionally left blank
        }
    }
}
