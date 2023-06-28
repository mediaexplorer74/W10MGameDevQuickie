using Microsoft.Xna.Framework;

namespace GameManager
{

    public interface ITargetable
    {
        Rectangle Rectangle { get; }
        Vector2 Position { get; set; }

        void RegisterTargetable();

        //        void RegisterTargetable()
        //        {
        //            DragDropManager.AddTarget(this);
        //        }
    }
}
