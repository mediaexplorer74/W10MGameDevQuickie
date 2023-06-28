using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameManager
{

    public class GameManager
    {
        private readonly Map _map;
        private readonly Hero _hero;
        private Matrix _translation;

        public GameManager()
        {
            _map = new();
            _hero = new(Glob.Content.Load<Texture2D>("hero"), 
                new(Glob.WindowSize.X / 2, Glob.WindowSize.Y / 2));

            _hero.SetBounds(_map.MapSize, _map.TileSize);
        }

        private void CalculateTranslation()
        {
            var dx = (Glob.WindowSize.X / 2) - _hero.Position.X;
            dx = MathHelper.Clamp(dx, -_map.MapSize.X 
                + Glob.WindowSize.X + (_map.TileSize.X / 2), _map.TileSize.X / 2);

            var dy = (Glob.WindowSize.Y / 2) - _hero.Position.Y;
            dy = MathHelper.Clamp(dy, -_map.MapSize.Y + Glob.WindowSize.Y 
                + (_map.TileSize.Y / 2), _map.TileSize.Y / 2);
            _translation = Matrix.CreateTranslation(dx, dy, 0f);
        }

        public void Update()
        {
            InputManager.Update();
            _hero.Update();
            CalculateTranslation();
        }

        public void Draw()
        {
            Glob.SpriteBatch.Begin(transformMatrix: _translation);
            _map.Draw();
            _hero.Draw();
            Glob.SpriteBatch.End();
        }
    }
}
