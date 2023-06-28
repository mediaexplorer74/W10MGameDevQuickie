using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Socket : Sprite, ITargetable
    {
        public Socket(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            (this as ITargetable).RegisterTargetable();
        }

        public void RegisterTargetable()
        {
            //throw new System.NotImplementedException();
            DragDropManager.AddTarget(this);
        }
    }
}
