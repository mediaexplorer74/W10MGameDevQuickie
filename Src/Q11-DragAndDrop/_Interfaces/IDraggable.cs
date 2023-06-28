using Microsoft.Xna.Framework;

namespace GameManager
{

    public interface IDraggable
    {
        Rectangle Rectangle { get; }
        Vector2 Position { get; set; }

        void RegisterDraggable();

        //void RegisterDraggable()
        //{
        //    DragDropManager.AddDraggable(this);
        //}
    }
}
