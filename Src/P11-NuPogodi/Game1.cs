// Game1

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameManager.AppLogic;
using GameManager.AppLogic.Components;
using GameManager.AppLogic.Model;
using GameManager.AppLogic.Model.Enums;
using GameComponent = GameManager.AppLogic.Components.GameComponent;
using Windows.UI.Xaml.Navigation;
using Microsoft.Xna.Framework.Input;

namespace GameManager
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;


        //private Timer timer;

        //private readonly GraphicsDeviceManager graphics;
        private GameController gameController;

        private ScoreComponent scoreComponent;
        private LevelComponent levelComponent;

        private readonly List<GameComponent> Components 
            = new List<GameComponent>();

        private readonly List<ButtonComponent> ButtonComponents 
            = new List<ButtonComponent>();

        public Game1()
        {
           
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
       

            // Get the content manager from the application
            //Content = ((App)Application.Current).Content;
            //Services = ((App)Application.Current).Services;

            // Create a timer for this page, 1s = 10 000 000 ticks
            //timer = new Timer 
            //{ 
            //    UpdateInterval = TimeSpan.FromTicks(1000000) 
            //};
            //timer.Update += OnUpdate;
            //timer.Draw += OnDraw;

            //Common.W = graphics.PreferredBackBufferWidth = 800;
            //Common.H = graphics.PreferredBackBufferHeight = 480;

            // Enable gestures
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Flick;
        }


        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;

            _graphics.ApplyChanges();

            //RnD
            Common.W = _graphics.PreferredBackBufferWidth;
            Common.H = _graphics.PreferredBackBufferHeight;


            Glob.Content = Content;

            _gameManager = new GameManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //TEST
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //RnD ***************************

            GraphicsDevice device = _graphics.GraphicsDevice;

            // create button component
            // set preview param to "true" if you want to debug button draw  
            CreateButtons(false, device);

            // create score component
            scoreComponent = new ScoreComponent(Content, device);
            Components.Add(scoreComponent);

            // create the main game controller
            gameController = new GameController(Content);

            // Set the sharing mode of the graphics device to turn on XNA rendering
            //GraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
           

            // load your game content here
            foreach (ButtonComponent button in ButtonComponents)
            {
                button.Initialize();
                button.LoadContent();
            }

            scoreComponent.Initialize();
            scoreComponent.LoadContent();

            gameController.LoadStartScreen();

            levelComponent = new LevelComponent(
                new ContentManager(Services, "Content"), device);

            Components.Insert(0, levelComponent);

            levelComponent.Initialize();
            levelComponent.LoadContent();

            //timer.Stop();
            //timer.Start();

            //RnD
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            

            // clean all unwanted input gestures from buffer
            while (TouchPanel.IsGestureAvailable)
            {
                TouchPanel.ReadGesture();
            }
            // ******************************

            //_spriteBatch = new SpriteBatch(GraphicsDevice);
            Glob.SpriteBatch = _spriteBatch;

        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            // --------------------
            //RnD
            //GameTime gameTime = new GameTime();//(e.TotalTime, e.ElapsedTime);

            //TouchPanel "handler"
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                
                switch (gesture.GestureType)
                {
                    case GestureType.Tap:
                ButtonComponent bc = ButtonComponents.FirstOrDefault
                (
                    b =>
                    {
                        return b.IsTap
                        (
                            (int)gesture.Position.X, 
                            (int)gesture.Position.Y
                        );
                    });
                if (bc != null)
                {
                    bc.Tap();
                }
                       break;
                    default:
                        break;
                }
            }


            //Mouse handler 
            if (InputManager.MouseLeftClicked)
            {
                
                MouseState mouseState = InputManager._lastMouseState;
                
                
                ButtonComponent bc = ButtonComponents.FirstOrDefault
                (
                    b =>
                    {
                        return b.IsTap
                        (
                            (int)mouseState.Position.X,//gesture.Position.X, 
                            (int)mouseState.Position.Y//gesture.Position.Y
                        );
                    });
                if (bc != null)
                {
                    bc.Tap();
                }
               
            }

            // update the level
            if (levelComponent != null)
            {
                levelComponent.Update(gameTime);
            }
            // --------------------

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Glob.Update(gameTime);

            _gameManager.Update();

            base.Update(gameTime);
        }//Update

        
        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // ************************
            
            //GameTime gameTime = new GameTime();//(e.TotalTime, e.ElapsedTime);
            foreach (GameComponent component in Components)
            {
                component.Draw(gameTime);
            }

            foreach (ButtonComponent component in ButtonComponents)
            {
                component.Draw(gameTime);
            }
            // ************************

            _gameManager.Draw();
                        
            _spriteBatch.End();

            base.Draw(gameTime);
        }//Draw



        // CreateButtons 
        private void CreateButtons(bool preview, GraphicsDevice device)
        {
            
            // direction buttons
            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonUpperLeft,
                Callback = () => GameController.I.Move(Direction.UpperLeft),
                Preview = preview,
            });

            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonBottomLeft,
                Callback = () => GameController.I.Move(Direction.BottomLeft),
                Preview = preview,
            });

            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonUpperRight,
                Callback = () => GameController.I.Move(Direction.UpperRight),
                Preview = preview,
            });

            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonBottomRight,
                Callback = () => GameController.I.Move(Direction.BottomRight),
                Preview = preview,
            });

            // game mode buttons
            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonModeA,
                Callback = () => GameController.I.LoadGame(GameMode.ModeA),
                Preview = preview,
            });

            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonModeB,
                Callback = () => GameController.I.LoadGame(GameMode.ModeB),
                Preview = preview,
            });

            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonAbout,
                Callback = () => GameController.I.LoadAbout(),
                Preview = preview,
            });

            // misc buttons
            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonSound,
                Callback = () => GameController.I.ToggleSound(),
                Preview = preview,
            });
            

            ButtonComponents.Add(new ButtonComponent(Content, device)
            {
                Rectangle = GameButtons.ButtonClock,
                Callback = () => GameController.I.LoadStartScreen(),
                Preview = preview,
            });

        }//CreateButtons
   
    }//GamePage class end

}//gameManager namespace end

