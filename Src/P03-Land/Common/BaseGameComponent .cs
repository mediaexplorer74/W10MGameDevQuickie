using Microsoft.Xna.Framework;

namespace GameManager.Common
{
    public class BaseGameComponent : GameComponent
    {
        public BaseGameComponent(Game1 game)
            : base(game)
        {
        }

        public new Game1 Game
        {
            get { return base.Game as Game1; }
        }
    }
}