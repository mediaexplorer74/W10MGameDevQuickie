using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class Gem : Sprite, IDraggable
    {
        public Gem(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            (this as IDraggable).RegisterDraggable();
        }

        public void RegisterDraggable()
        {
            //throw new System.NotImplementedException();
            DragDropManager.AddDraggable(this);
        }
    }
}
