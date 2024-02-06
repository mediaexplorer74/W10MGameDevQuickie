using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager.Views
{
    /// <summary>
    /// Base view class.
    /// </summary>
    abstract class BaseView
    {
        /// <summary>
        /// The content manager.
        /// </summary>
        public ContentManager ContentManager { get; }

        /// <summary>
        /// The sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contentManager">Content manager</param>
        /// <param name="spriteBatch">Sprite batch</param>
        public BaseView(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            ContentManager = contentManager;
            SpriteBatch = spriteBatch;
        }

        public abstract void Draw();
    }
}
