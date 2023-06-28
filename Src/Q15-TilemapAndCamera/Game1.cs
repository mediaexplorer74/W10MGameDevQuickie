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
            Glob.WindowSize = new(/*1024*/Glob.Width, /*768*/Glob.Height);

            _graphics.PreferredBackBufferWidth = Glob.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Glob.WindowSize.Y;
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

            _gameManager.Draw();

            base.Draw(gameTime);
        }
    }
}