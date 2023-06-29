namespace GameManager.Library.Game.GameObjects.Characters
{
    public class BaseCharacterObject
    {
        public ElementContainer container;
        public string Name;

        public BaseCharacterObject(ElementContainer container)
        {
            this.container = container;
        }
    }
}