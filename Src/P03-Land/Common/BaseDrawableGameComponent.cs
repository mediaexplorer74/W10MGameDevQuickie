using Microsoft.Xna.Framework;

namespace GameManager.Common
{
    public class BaseDrawableGameComponent : DrawableGameComponent
    {
        public BaseDrawableGameComponent(Game1 game)
            : base(game)
        {
        }

        public new Game1 Game
        {
            get { return base.Game as Game1; }
        }

        public virtual void Show(bool value)
        {
            Enabled = value;
            Visible = value;
        }
    }
}