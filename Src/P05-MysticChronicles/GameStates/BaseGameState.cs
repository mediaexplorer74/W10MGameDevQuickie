namespace GameManager.Library.Game.GameStates
{
    public class BaseGameState
    {
        private GameStateContainer container;

        public BaseGameState(GameStateContainer container)
        {
            this.container = container;
        }
    }
}