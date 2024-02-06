//-----------------------------------------------------------------------------
// NeonShooterGame.cs
//-----------------------------------------------------------------------------


using BloomPostprocess;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

#if !__IOS__
using Microsoft.Xna.Framework.Media;
#endif

namespace NeonShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class NeonShooterGame: Microsoft.Xna.Framework.Game
    {
        // Resources for drawing.
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Vector2 baseScreenSize = new Vector2(800, 480);
        private Matrix globalTransformation;
        int backbufferWidth, backbufferHeight;

        // some helpful static properties
        public static NeonShooterGame Instance { get; private set; }
		public static Viewport Viewport 
        { 
            get 
            { 
                return Instance.GraphicsDevice.Viewport; 
            } 
        }

		public static Vector2 ScreenSize 
        { 
            get 
            { 
                return new Vector2
                (
                    800/*Viewport.Width*/, 
                    480/*Viewport.Height*/
               ); 
            } 
        }
		public static GameTime GameTime { get; private set; }
		public static ParticleManager<ParticleState> ParticleManager { get; private set; }
		public static Grid Grid { get; private set; }

		//GraphicsDeviceManager graphics;
		//SpriteBatch spriteBatch;
		BloomComponent bloom;

		bool paused = false;

        
        public NeonShooterGame()
        {
			Instance = this;
			graphics = new GraphicsDeviceManager(this);
#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif
            
            graphics.PreferredBackBufferWidth = 800;//1920;
            graphics.PreferredBackBufferHeight = 480;//1080;
            
            graphics.IsFullScreen = true; // set it true - for W10M , false - for W11 desktop debug

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft
               | DisplayOrientation.LandscapeRight;// | DisplayOrientation.Portrait;

            bloom = new BloomComponent(this);
			Components.Add(bloom);
			bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);
            bloom.Visible = false;
        }

        protected override void Initialize()
        {
            this.Content.RootDirectory = "Content";

            //RnD
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ParticleManager = new ParticleManager<ParticleState>
                (1024 * 20, 
                ParticleState.UpdateParticle
                );

            const int maxGridPoints = 1600;
            Vector2 gridSpacing = new Vector2((float)Math.Sqrt(
                Viewport.Width * Viewport.Height / maxGridPoints));
            Grid = new Grid(Viewport.Bounds, gridSpacing);

            //RnD
            ScalePresentationArea();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Art.Load(Content);
			Sound.Load(Content);

            EntityManager.Add(PlayerShip.Instance);


#if !__IOS__
            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(Sound.Music);
            }
            catch { }
#endif
        }//LoadContent


        //RnD
        public void ScalePresentationArea()
        {
            //Work out how much we need to scale our graphics to fill the screen
            backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth - 0; // 40 - dirty hack for Astoria!
            backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

            float horScaling = backbufferWidth / baseScreenSize.X;
            float verScaling = backbufferHeight / baseScreenSize.Y;

            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);

            globalTransformation = Matrix.CreateScale(screenScalingFactor);

            System.Diagnostics.Debug.WriteLine("Screen Size - Width["
                + GraphicsDevice.PresentationParameters.BackBufferWidth + "] " +
                "Height [" + GraphicsDevice.PresentationParameters.BackBufferHeight + "]");
        }




        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Confirm the screen has not been resized by the user
            if (backbufferHeight != GraphicsDevice.PresentationParameters.BackBufferHeight ||
                backbufferWidth != GraphicsDevice.PresentationParameters.BackBufferWidth)
            {
                ScalePresentationArea();
            }


            GameTime = gameTime;
            Input.Update();

#if !__IOS__
            // Allows the game to exit
            if (Input.WasButtonPressed(Buttons.Back) || Input.WasKeyPressed(Keys.Escape))
                this.Exit();
#endif

            if (Input.WasKeyPressed(Keys.P))
                paused = !paused;
            if (Input.WasKeyPressed(Keys.B))
                bloom.Visible = !bloom.Visible;

            if (!paused)
            {
                PlayerStatus.Update();
                EntityManager.Update();
                EnemySpawner.Update();
                ParticleManager.Update();
                Grid.Update();
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            bloom.BeginDraw();

            GraphicsDevice.Clear(Color.Black);

            //RnD
            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive,//); 
                null, null, null, null, globalTransformation);
            
            EntityManager.Draw(spriteBatch);
            
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,//);
                 null, null, null, null, globalTransformation);

            Grid.Draw(spriteBatch);
            
            ParticleManager.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);

            // Draw the user interface without bloom
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive,//);
                 null, null, null, null, globalTransformation);

            DrawTitleSafeAlignedString("Lives: " + PlayerStatus.Lives, 5);
            DrawTitleSafeRightAlignedString("Score: " + PlayerStatus.Score, 5);
            DrawTitleSafeRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);
            // draw the custom mouse cursor
            spriteBatch.Draw(Art.Pointer, Input.MousePosition, Color.White);

            if (PlayerStatus.IsGameOver)
            {
                string text = "Game Over\n" +
                    "Your Score: " + PlayerStatus.Score + "\n" +
                    "High Score: " + PlayerStatus.HighScore;

                Vector2 textSize = Art.Font.MeasureString(text);
                spriteBatch.DrawString(Art.Font, text, ScreenSize / 2 - textSize / 2, 
                    Color.White);
            }

            spriteBatch.End();
        }

        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font.MeasureString(text).X;
            spriteBatch.DrawString(Art.Font, text, 
                new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }

        private void DrawTitleSafeAlignedString(string text, int pos)
        {
            spriteBatch.DrawString(Art.Font, text, 
                new Vector2(Viewport.TitleSafeArea.X + pos), Color.White);
        }

        private void DrawTitleSafeRightAlignedString(string text, float y)
        {
            var textWidth = Art.Font.MeasureString(text).X;
            spriteBatch.DrawString(Art.Font, text, 
                new Vector2(ScreenSize.X - textWidth - 5 - Viewport.TitleSafeArea.X, 
                Viewport.TitleSafeArea.Y + y), Color.White);
        }
    }
}
