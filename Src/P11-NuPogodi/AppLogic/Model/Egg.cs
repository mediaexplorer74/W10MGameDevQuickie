using GameManager.AppLogic.Model.Enums;

namespace GameManager.AppLogic.Model
{
    public class Egg : Item
    {
        public Direction Direction { get; set; }
        public int State { get; set; }
    }
}
