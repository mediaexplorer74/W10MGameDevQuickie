using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Windows.UI.Input;

namespace DialPong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<RadialControllerMenuItem> menuItems;

        Paddle paddle;
        Ball ball;

        bool inFocus;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            inFocus = false;
            menuItems = new List<RadialControllerMenuItem>();
            World.dial.RotationResolutionInDegrees = 1;
            World.dial.UseAutomaticHapticFeedback = true;
            RadialControllerMenuItem paddleMenuItem = RadialControllerMenuItem.CreateFromKnownIcon("Paddle", RadialControllerMenuKnownIcon.Ruler);
            RadialControllerMenuItem sensitivityMenuItem = RadialControllerMenuItem.CreateFromKnownIcon("Sensitivity", RadialControllerMenuKnownIcon.Scroll);
            RadialControllerMenuItem exitMenuItem = RadialControllerMenuItem.CreateFromKnownIcon("Exit", RadialControllerMenuKnownIcon.NextPreviousTrack);
            exitMenuItem.Invoked += ExitMenuItem_Invoked;
            menuItems.Add(paddleMenuItem);
            menuItems.Add(sensitivityMenuItem);
            menuItems.Add(exitMenuItem);
            for (int i = 0; i < menuItems.Count; i++)
            {
                RadialControllerMenuItem item = menuItems[i];
                int index = i;
                World.dial.Menu.Items.Add(item);
            }
            World.dialConfig.SetDefaultMenuItems(new List<RadialControllerSystemMenuItemKind>());
            World.dial.RotationChanged += Dial_RotationChanged;
            World.dial.ButtonClicked += Dial_ButtonClicked;
            World.dial.ControlAcquired += Dial_ControlAcquired;
            World.dial.ControlLost += Dial_ControlLost;
        }

        private void ExitMenuItem_Invoked(RadialControllerMenuItem sender, object args)
        {
            this.Exit();
        }

        private void Dial_ControlLost(RadialController sender, object args)
        {
            inFocus = false;
            World.dial.RotationResolutionInDegrees = 1;
        }

        private void Dial_ControlAcquired(RadialController sender, RadialControllerControlAcquiredEventArgs args)
        {
            inFocus = true;
        }

        private void Dial_ButtonClicked(RadialController sender, RadialControllerButtonClickedEventArgs args)
        {
            var selected = World.dial.Menu.GetSelectedMenuItem();
            if (selected.DisplayText == "Exit")
            {
                this.Exit();
            }
            else
            {
                if (ball != null)
                {
                    if (ball.sprite.velocity == Vector2.Zero)
                    {
                        ball.Launch();
                    }
                }
            }
        }

        private void Dial_RotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs args)
        {
            var selected = World.dial.Menu.GetSelectedMenuItem();
            if (selected.DisplayText == "Paddle")
            {
                if (paddle != null)
                {
                    paddle.Move((float)args.RotationDeltaInDegrees);
                }
            }
            else if (selected.DisplayText == "Sensitivity")
            {
                if (paddle != null)
                {
                    paddle.ChangeSensitivity((float)args.RotationDeltaInDegrees);
                }
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            paddle = new Paddle(graphics);
            ball = new Ball(graphics);
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            if (inFocus)
            {
                ball.Update();
                paddle.Update();
                ball.HandlePaddleBounce(paddle);
            }

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
            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
