namespace GameManager
{
    public abstract class GameState
    {
        public abstract void Update(GameManager gm);
        public abstract void Draw(GameManager gm);
    }
}
