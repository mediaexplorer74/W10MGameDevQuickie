using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class GameManager
    {
        private readonly Map _map;
        private readonly Hero _hero;

        public GameManager()
        {
            _map = new();
            _hero = new(Glob.Content.Load<Texture2D>("hero"), 
                new(Glob.WindowSize.X / 2, 200));
        }

        public void Update()
        {
            _hero.Update();
        }

        public void Draw()
        {
            Glob.SpriteBatch.Begin();
            _map.Draw();
            _hero.Draw();
            Glob.SpriteBatch.End();
        }
    }
}
