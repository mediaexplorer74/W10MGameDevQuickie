using Microsoft.Xna.Framework;

namespace GameManager
{

    public abstract class TrailStrategy
    {
        public abstract bool Ready(Vector2 position);
    }
}
