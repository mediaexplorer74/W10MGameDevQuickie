namespace GameManager.Library.Game.GameObjects.Characters
{
    internal class PartyMember : BaseGraphicElement
    {
        private object container;
        private string v;
        private int partyIndex;

        public PartyMember(object container, string v, int partyIndex)
        {
            this.container = container;
            this.v = v;
            this.partyIndex = partyIndex;
        }
    }
}