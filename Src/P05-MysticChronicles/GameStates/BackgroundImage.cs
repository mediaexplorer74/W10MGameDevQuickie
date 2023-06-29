using GameManager.Library.Game.GameObjects.Characters;
using GameManager.Library.Game.GameObjects.GUI;

namespace GameManager.Library.Game.GameStates
{
    internal class BackgroundImage
    {
        private object eContainer;
        private string v;
        private ElementContainer container;
        private object value;

        public BackgroundImage(object value, string v)
        {
            this.value = value;
            this.v = v;
        }



        /*
        public BackgroundImage(var eContainer, string v)
        {
            this.eContainer = eContainer;
            this.v = v;
        }
        */
    }
}