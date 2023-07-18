using Microsoft.Xna.Framework;

namespace GameManager.AppLogic.Components
{
    public abstract class GameComponent
    {
        public abstract void Draw(GameTime gameTime);
        public abstract void Update(GameTime gameTime);
        public abstract void Initialize();
        public abstract void LoadContent();
        public abstract void UnloadContent();
    }
}
