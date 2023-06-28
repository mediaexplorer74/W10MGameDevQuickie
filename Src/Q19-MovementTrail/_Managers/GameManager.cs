using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class GameManager
    {
        private readonly Hero _hero;

        public GameManager()
        {
            _hero = new(Glob.Content.Load<Texture2D>("hero"), new(100, 100));
        }

        public void Update()
        {
            InputManager.Update();
            _hero.Update();
        }

        public void Draw()
        {
            Glob.SpriteBatch.Begin();
            _hero.Draw();
            Glob.SpriteBatch.End();
        }
    }
}
