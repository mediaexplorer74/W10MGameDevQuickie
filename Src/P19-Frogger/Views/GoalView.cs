using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.General;
using GameManager.Models;

namespace GameManager.Views
{
    /// <summary>
    /// Draw the frogs in their homes.
    /// </summary>
    class GoalView : BaseView
    {
        private readonly SpriteSheet _blocks;
        private readonly GoalContainerModel _goals;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="contentManager">Content manager</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="goals">Goal model</param>
        public GoalView(ContentManager contentManager, SpriteBatch spriteBatch, GoalContainerModel goals)
            : base(contentManager, spriteBatch)
        {
            var blocksTexture = contentManager.Load<Texture2D>("blocks");
            _blocks = new SpriteSheet(blocksTexture, spriteBatch, 16, 16);
            _goals = goals;
        }

        /// <summary>
        /// Draw the frogs in their homes.
        /// </summary>
        public override void Draw()
        {
            foreach (var goal in _goals.Goals)
            {
                if (goal.Occupied)
                    _blocks.Draw(new Vector2(goal.Area.Left, goal.Area.Top), 10 + goal.Frame, Color.White);
            }
        }
    }
}
