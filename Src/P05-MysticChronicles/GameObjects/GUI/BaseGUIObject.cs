using GameManager.Library.Game.GameObjects.Characters;

namespace GameManager.Library.Game.GameObjects.GUI
{
    public class BaseGUIObject
    {
        private ElementContainer container;

        public BaseGUIObject(ElementContainer container)
        {
            this.container = container;
        }
    }
}