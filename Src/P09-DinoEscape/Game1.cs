using GameManager.JRPG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Tesserae;

namespace GameManager
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;

        private GameManager _gameManager;

        //Experimental
        Zone _zoneTest;
        Zone zZone;

        public SpriteBatch spriteBatch;

        public Mosaic mosaic;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            // Default window resolution
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = Glob.defaultWidth;
            _graphics.PreferredBackBufferHeight = Glob.defaultHeight;
            _graphics.ApplyChanges();

            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Glob.defaultWidth;//1024;
            _graphics.PreferredBackBufferHeight = Glob.defaultHeight;//768;
            _graphics.ApplyChanges();

            Glob.Content = Content;

            _zoneTest = new();
            
            

            //_gameManager = new();
            //_gameManager.Init();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mosaic = new Mosaic(this, "Content/sewers.tmx");

            //RnD
            zZone = _zoneTest.Load(mosaic.map, Content);


            Glob.SpriteBatch = spriteBatch;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Glob.Update(gameTime);
            // _gameManager.Update();

            zZone.UpdatePosition(Keyboard.GetState(), gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            /*
             GraphicsDevice.Clear(Color.Beige);

            _spriteBatch.Begin();
            _gameManager.Draw();
            _spriteBatch.End();

            */

            //base.Draw(gameTime);

            // ****************************************************************
            // RnD zone
            // Todo : Add your drawing code here


            // * Mosaic Canvas * Experimental thing 1 :)
            //mosaic.DrawCanvas(spriteBatch);

            // Draw on the back buffer
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                              SamplerState.PointClamp, null, null);

           

            // * JRPG * Experimental thing 2 too :)
            zZone.Draw(spriteBatch);

            // * MosaicRect * Experimental thing 3
            //var rect = new Rectangle(0, 0, Glob.defaultWidth, Glob.defaultHeight); 
            //spriteBatch.Draw(mosaic.renderTarget, rect, /*Color.White*/Color.Red);
            
            //_gameManager.Draw();

            spriteBatch.End();
            
            // ****************************************************************
            //RnD : place it before *** 
            
            base.Draw(gameTime);

            
        }
    }
}
