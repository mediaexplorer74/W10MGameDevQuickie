namespace GameManager
{

    public class GameManager
    {
        private readonly Map _map = new();

        public void Update()
        {
            InputManager.Update();
            _map.Update();
        }

        public void Draw()
        {
            _map.Draw();
        }
    }
}