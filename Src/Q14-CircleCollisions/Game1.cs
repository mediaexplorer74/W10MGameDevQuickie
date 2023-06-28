using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameManager
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            Glob.Bounds = new(Glob.Width, Glob.Height);

            _graphics.PreferredBackBufferWidth = Glob.Bounds.X;
            _graphics.PreferredBackBufferHeight = Glob.Bounds.Y;
            _graphics.ApplyChanges();

            Glob.Content = Content;

            _gameManager = new();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Glob.SpriteBatch = _spriteBatch;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Glob.Update(gameTime);
            _gameManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _gameManager.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}