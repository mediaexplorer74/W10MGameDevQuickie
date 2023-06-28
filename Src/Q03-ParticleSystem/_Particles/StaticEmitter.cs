using Microsoft.Xna.Framework;

namespace GameManager
{

    public class StaticEmitter : IEmitter
    {
        public Vector2 EmitPosition { get; }

        public StaticEmitter(Vector2 pos)
        {
            EmitPosition = pos;
        }
    }
}
