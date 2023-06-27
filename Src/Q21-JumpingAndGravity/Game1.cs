using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameManager
{

    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Glob.WindowSize = new(Map.tiles.GetLength(1) * Map.TILE_SIZE, 
                Map.tiles.GetLength(0) * Map.TILE_SIZE);
            _graphics.PreferredBackBufferWidth = Glob.WindowSize.X;
            _graphics.PreferredBackBufferHeight = Glob.WindowSize.Y;
            _graphics.ApplyChanges();

            Glob.Content = Content;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Glob.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Glob.GraphicsDevice = GraphicsDevice;

            _gameManager = new();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back 
                == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Glob.Update(gameTime);
            _gameManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.SkyBlue);

            _gameManager.Draw();

            base.Draw(gameTime);
        }
    }
}



/*

namespace GameManager
{
    public class Game1 : Game
    {
        // The ratio of the screen that is sky versus ground
        const float SKYRATIO = 2f / 3f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int score = 0;

        float screenWidth;
        float screenHeight;
        float broccoliSpeedMultiplier;
        float dinoSpeedX;
        float dinoJumpY;
        float gravitySpeed;

        bool spaceDown;
        bool gameStarted;
        bool gameOver;

        Texture2D grass;
        Texture2D startGameSplash;
        Texture2D gameOverTexture;

        SpriteClass dino;
        SpriteClass broccoli;
        
        Random random;

        SpriteFont scoreFont;
        SpriteFont stateFont;

        // Mouse support
        MouseState lastMouseState;
        Vector2 mouseposition = Vector2.Zero;

        // TouchPanel support
        TouchCollection lastTouchState;
        Vector2 touchposition = Vector2.Zero;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // Set the directory where game assets can be found by the ContentManager
            Content.RootDirectory = "Content";
        }


        // Give variables their initial states
        // Called once when the app is started
        protected override void Initialize()
        {
            base.Initialize();

            // Attempt to launch in fullscreen mode
            ApplicationView.PreferredLaunchWindowingMode
                = ApplicationViewWindowingMode.FullScreen;//.CompactOverlay//.Auto             

            // Get screen height and width, scaling them up if running on a high-DPI monitor.
            screenHeight = 
               ScaleToHighDPI(
                (float)ApplicationView.GetForCurrentView().VisibleBounds.Height+100);

            screenWidth = 
               ScaleToHighDPI(
                (float)ApplicationView.GetForCurrentView().VisibleBounds.Width+100);

            broccoliSpeedMultiplier = 0.01f;// 0.25f;//0.5f;

            spaceDown = false;
            gameStarted = false;
            gameOver = false;

            random = new Random();

            dinoSpeedX =  ScaleToHighDPI(500f);//1000
            dinoJumpY = ScaleToHighDPI(-600f);//-1200
            gravitySpeed =  ScaleToHighDPI(12f);//30
            score = 0;

            this.IsMouseVisible = true;//false; // Hide the mouse within the app window
        }


        // Load content (eg.sprite textures) before the app runs
        // Called once when the app is started
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            grass = Content.Load<Texture2D>("grass");
            startGameSplash = Content.Load<Texture2D>("start-splash");
            gameOverTexture = Content.Load<Texture2D>("game-over");

            // Construct SpriteClass objects
            dino = new SpriteClass
            (
                GraphicsDevice, 
                "Content/ninja-cat-dino.png", 
                ScaleToHighDPI(1f)
            );

            broccoli = new SpriteClass
            (
                GraphicsDevice, 
                "Content/broccoli.png", 
                ScaleToHighDPI(0.2f)
            );

            // Load fonts
            scoreFont = Content.Load<SpriteFont>("Score");
            stateFont = Content.Load<SpriteFont>("GameState");
        }


        // Unloads any non ContentManager content
        protected override void UnloadContent()
        {
            //
        }


        // Updates the logic of the game state each frame,
        // checking for collision, gathering input, etc.
        protected override void Update(GameTime gameTime)
        {
            //RnD

            // Get screen height and width, scaling them up if running on a high-DPI monitor.
            screenHeight =
               ScaleToHighDPI(
                (float)ApplicationView.GetForCurrentView().VisibleBounds.Height);

            screenWidth =
               ScaleToHighDPI(
                (float)ApplicationView.GetForCurrentView().VisibleBounds.Width);

            // Get time elapsed since last Update iteration
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            MouseAndKeyboardHandler(); // Handle mouse&keyboard input

            // Stop all movement when the game ends
            if (gameOver)
            {
                dino.dX = 0;
                dino.dY = 0;
                broccoli.dX = 0;
                broccoli.dY = 0;
                broccoli.dA = 0;
            }

            // Update animated SpriteClass objects based on their current rates of change
            dino.Update(elapsedTime);
            broccoli.Update(elapsedTime);

            // Accelerate the dino downward each frame to simulate gravity.
            dino.dY += gravitySpeed;

            // Set game floor
            if (dino.y > screenHeight * SKYRATIO)
            {
                dino.dY = 0;
                dino.y = screenHeight * SKYRATIO;
            }

            // Set right edge
            if (dino.x > screenWidth - dino.texture.Width / 2)
            {
                dino.x = screenWidth - dino.texture.Width / 2;
                dino.dX = 0;
            }

            // Set left edge
            if (dino.x < 0 + dino.texture.Width / 2)
            {
                dino.x = 0 + dino.texture.Width / 2;
                dino.dX = 0;
            }

            // If the broccoli goes offscreen, spawn a new one and iterate the score
            if 
            (
                broccoli.y > screenHeight + 100 
                || broccoli.y < -100 
                || broccoli.x > screenWidth + 100 
                || broccoli.x < -100
            )
            {
                SpawnBroccoli();
                score++;
            }

            if (dino.RectangleCollision(broccoli)) gameOver = true; 
            // End game if the dino collides with the broccoli

            base.Update(gameTime);
        }


        // Draw the updated game state each frame
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Begin drawing
            spriteBatch.Begin(); 

            // Draw grass
            spriteBatch.Draw
            (
                grass, 
                new Rectangle(0, (int)(screenHeight * SKYRATIO), 
                (int)screenWidth, 
                (int)screenHeight), 
                Color.White
            );

            if (gameOver)
            {
                // Draw game over texture
                spriteBatch.Draw
                (
                    gameOverTexture, 
                    new Vector2(screenWidth / 2 - gameOverTexture.Width / 2, 
                    screenHeight / 4 - gameOverTexture.Width / 2), 
                    Color.White
                );

                String pressEnter = "Press Enter to restart!";

                // Measure the size of text in the given font
                Vector2 pressEnterSize = stateFont.MeasureString(pressEnter);

                // Draw the text horizontally centered
                spriteBatch.DrawString
                (
                    stateFont, 
                    pressEnter, 
                    new Vector2(screenWidth / 2 - pressEnterSize.X / 2, 
                    screenHeight - 200), 
                    Color.White
                );

                // If the game is over, draw the score in red
                spriteBatch.DrawString
                (
                    scoreFont, 
                    score.ToString(), 
                    new Vector2(screenWidth - 100, 50), 
                    Color.Red
                );
            }

            // If the game is not over, draw it in black
            else spriteBatch.DrawString(scoreFont, score.ToString(), 
                new Vector2(screenWidth - 100, 50), 
                Color.Black);

            // Draw broccoli and dino with the SpriteClass method
            broccoli.Draw(spriteBatch);
            dino.Draw(spriteBatch);

            if (!gameStarted)
            {
                // Fill the screen with black before the game starts
                spriteBatch.Draw(startGameSplash, 
                    new Rectangle(0, 0, (int)screenWidth, (int)screenHeight), 
                    Color.White);

                String title = "VEGGIE JUMP";
                String pressSpace = "Press Space to start";

                // Measure the size of text in the given font
                Vector2 titleSize = stateFont.MeasureString(title);
                Vector2 pressSpaceSize = stateFont.MeasureString(pressSpace);

                // Draw the text horizontally centered
                spriteBatch.DrawString
                (
                    stateFont, 
                    title, 
                    new Vector2(screenWidth / 2 - titleSize.X / 2, screenHeight / 3), 
                    Color.ForestGreen
                );

                spriteBatch.DrawString
                (
                    stateFont, 
                    pressSpace, 
                    new Vector2(screenWidth / 2 - pressSpaceSize.X / 2, screenHeight / 2), 
                    Color.White
                );
            }

            spriteBatch.End(); // Stop drawing

            base.Draw(gameTime);
        }


        // Scale a number of pixels so that it displays properly
        // on a High-DPI screen, such as a Surface Pro or Studio
        public float ScaleToHighDPI(float f)
        {
            DisplayInformation d = DisplayInformation.GetForCurrentView();
            f *= (float)d.RawPixelsPerViewPixel;
            return f;
        }


        // Spawn the broccoli object in a random location offscreen
        public void SpawnBroccoli()
        {
            // Spawn broccoli either left (1), above (2), right (3),
            // or below (4) the screen
            int direction = random.Next(1, 5);
            switch (direction)
            {
                case 1:
                    broccoli.x = -100;
                    broccoli.y = random.Next(0, (int)screenHeight);
                    break;
                case 2:
                    broccoli.y = -100;
                    broccoli.x = random.Next(0, (int)screenWidth);
                    break;
                case 3:
                    broccoli.x = screenWidth + 100;
                    broccoli.y = random.Next(0, (int)screenHeight);
                    break;
                case 4:
                    broccoli.y = screenHeight + 100;
                    broccoli.x = random.Next(0, (int)screenWidth);
                    break;
            }

            // Increase game difficulty (ie broccoli speed) for every five points scored
            if (score % 5 == 0) broccoliSpeedMultiplier += 0.1f;//0.2f;

            // Orient the broccoli sprite towards the dino sprite and set angular velocity
            broccoli.dX = (dino.x - broccoli.x) * broccoliSpeedMultiplier;
            broccoli.dY = (dino.y - broccoli.y) * broccoliSpeedMultiplier;
            broccoli.dA = 7f;
        }


        // Start a new game, either when the app starts up or after game over
        public void StartGame()
        {
            // Reset dino position
            dino.x = screenWidth / 2;
            dino.y = screenHeight * SKYRATIO;

            // Reset broccoli speed and respawn it
            broccoliSpeedMultiplier = 0.02f;// 0.25f;//0.5f;
            SpawnBroccoli();

            score = 0; // Reset score
                       
            spaceDown = true; // =)
        }


        // Handle user input from the keyboard
        void MouseAndKeyboardHandler()
        {
            // ++++++++++++++ TOUCH PANEL ++++++++++++++++++++

            TouchCollection currentTouchState = TouchPanel.GetState();

            if (currentTouchState.Count > 0)
            {
                if (currentTouchState[0].Position.X != lastTouchState[0].Position.X ||
                currentTouchState[0].Position.Y != lastTouchState[0].Position.Y)
                {
                    //DEBUG
                    touchposition = new Vector2(currentTouchState[0].Position.X,
                        currentTouchState[0].Position.Y);

                    if (currentTouchState[0].Position.X < lastTouchState[0].Position.X)
                    {
                        dino.dX = dinoSpeedX * -1;
                    }
                    else if (currentTouchState[0].Position.X > lastTouchState[0].Position.X)
                    {
                        dino.dX = dinoSpeedX * 1;
                    }
                    else
                    {
                        dino.dX = 0;
                    }
                }//if
            }//if (currentTouchState.Count...

            // +++++++++++++++++++++++++++++++++++++++++++++++

            // ************** MOUSE **************************
            MouseState currentMouseState = Mouse.GetState();

            if (currentMouseState.X != lastMouseState.X ||
                currentMouseState.Y != lastMouseState.Y)

            {
                //DEBUG
                mouseposition = new Vector2(currentMouseState.X, currentMouseState.Y);

                if (currentMouseState.X < lastMouseState.X)
                {
                    dino.dX = dinoSpeedX * -1;
                }
                else if (currentMouseState.X > lastMouseState.X)
                {
                    dino.dX = dinoSpeedX * 1;
                }
                else
                {
                    dino.dX = 0;
                }
            }
            // **************************************************************

            // ====================== KEYBOARD ==============================
            KeyboardState state = Keyboard.GetState();

            // Quit the game if Escape is pressed.
            if (state.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Start the game if Space is pressed.
            // Exit the keyboard handler method early,
            // preventing the dino from jumping on the same keypress.
            if (!gameStarted)
            {
                if 
                (
                   currentTouchState[0].State == TouchLocationState.Pressed
                   ||
                   currentMouseState.LeftButton == ButtonState.Pressed
                   ||
                   state.IsKeyDown(Keys.Space)
                )
                {
                    StartGame();
                    gameStarted = true;
                    spaceDown = true;
                    gameOver = false;
                }
                return;
            }

            // Restart the game if Enter is pressed
            if (gameOver)
            {
               

                if 
                (
                   currentTouchState[0].State == TouchLocationState.Pressed
                   ||
                   currentMouseState.LeftButton == ButtonState.Pressed
                   ||
                   state.IsKeyDown(Keys.Enter)
                   ||
                   state.IsKeyDown(Keys.Space)
                )
                {
                    StartGame();
                    gameOver = false;
                }
            }

            // Jump if Space (or another jump key) is pressed
            if 
            (
                state.IsKeyDown(Keys.Space)
                || 
                //state.IsKeyDown(Keys.W)
                //|| 
                currentTouchState[0].State == TouchLocationState.Pressed
                ||
                currentMouseState.LeftButton == ButtonState.Pressed
                ||
                state.IsKeyDown(Keys.Up)
            )
            {
                // Jump if Space is pressed but not held and the dino is on the floor
                if 
                (
                    !spaceDown 
                    && 
                    dino.y >= screenHeight * SKYRATIO - 1
                )
                {
                    dino.dY = dinoJumpY;
                }

                spaceDown = true;
            }
            else
            {
                spaceDown = false;
            }

            // check "mouse & touchpanel sleep phase" 
            if (lastMouseState == currentMouseState
                &&
                currentTouchState.Count == 0
                )
            {
                // Handle left and right
                if (state.IsKeyDown(Keys.Left) )
                {
                    dino.dX = dinoSpeedX * -1;
                }
                else if (state.IsKeyDown(Keys.Right))
                {
                    dino.dX = dinoSpeedX;
                }
                else
                {
                    dino.dX = 0;
                }
            }

            // ====================================================

            lastMouseState = currentMouseState;

            lastTouchState = currentTouchState;
        }
    }
}//namespace

*/
