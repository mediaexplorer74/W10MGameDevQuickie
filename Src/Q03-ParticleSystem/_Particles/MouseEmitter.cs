using Microsoft.Xna.Framework;

namespace GameManager
{

    public class MouseEmitter : IEmitter
    {
        public Vector2 EmitPosition => InputManager.MousePosition;
    }
}
