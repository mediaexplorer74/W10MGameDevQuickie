using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Windows.UI.Input;

namespace CombinationLock
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Sprite circle;
        DialLock dialLock;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization 
        /// it needs to before starting to run.
        /// This is where it can query for any required services 
        /// and load any non-graphic related content.  
        /// Calling base.Initialize will enumerate 
        /// through any components and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            World.dial.RotationResolutionInDegrees = 2;
            World.dial.UseAutomaticHapticFeedback = false;

            RadialControllerMenuItem item = 
                RadialControllerMenuItem.CreateFromKnownIcon(
                    "lock", RadialControllerMenuKnownIcon.Ruler);

            World.dial.Menu.Items.Add(item);
            World.dialConfig.SetDefaultMenuItems(
                new List<RadialControllerSystemMenuItemKind>());

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

            circle = new Sprite(Content.Load<Texture2D>("circle"));
            
            circle.position = new Vector2(300);

            dialLock = new DialLock(Content.Load<Texture2D>("dial"), 
                Content.Load<Texture2D>("catch"));

            dialLock.Position = new Vector2(300);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            dialLock.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            circle.Draw(spriteBatch);
            dialLock.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
