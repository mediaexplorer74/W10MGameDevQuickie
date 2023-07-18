/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace GameManager
{
    public sealed partial class GamePage : Page
    {
		readonly Game1 _game;

		public GamePage()
        {
            this.InitializeComponent();

			// Create the game.
			var launchArguments = string.Empty;
            _game = MonoGame.Framework.XamlGame<Game1>.Create(
                launchArguments, 
                Window.Current.CoreWindow, 
                swapChainPanel);
        }
    }
}
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using GameManager.AppLogic;
using GameManager.AppLogic.Components;
using GameManager.AppLogic.Model;
using GameManager.AppLogic.Model.Enums;
using GameComponent = GameManager.AppLogic.Components.GameComponent;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace GameManager.Pages
{
    public partial class GamePage
    {
        private ContentManager Content;
        //private AppServiceProvider Services;
        private GameTimer timer;

        private readonly GraphicsDeviceManager graphics;
        private GameController gameController;

        private ScoreComponent scoreComponent;
        private LevelComponent levelComponent;

        private readonly List<GameComponent> Components = new List<GameComponent>();
        private readonly List<ButtonComponent> ButtonComponents = new List<ButtonComponent>();

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            //Content = ((App)Application.Current).Content;
            //Services = ((App)Application.Current).Services;

            // Create a timer for this page, 1s = 10 000 000 ticks
            timer = new GameTimer 
            { 
                UpdateInterval = TimeSpan.FromTicks(1000000) 
            };
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            //graphics = new GraphicsDeviceManager(this);
            graphics = GraphicsDeviceManager.Current;

            Common.W = graphics.PreferredBackBufferWidth = 800;
            Common.H = graphics.PreferredBackBufferHeight = 480;

            // Enable gestures
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Flick;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GraphicsDevice device = graphics.GraphicsDevice;

            // create button component
            CreateButtons(false, device);

            // create score component
            scoreComponent = new ScoreComponent(Content, device);
            Components.Add(scoreComponent);

            // create the main game controller
            gameController = new GameController(Content);

            // Set the sharing mode of the graphics device to turn on XNA rendering
            GraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

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
            timer.Stop();
            timer.Start();

            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // clean all unwanted input gestures from buffer
            while (TouchPanel.IsGestureAvailable)
            {
                TouchPanel.ReadGesture();
            }

            base.OnNavigatedTo(e);
        }

        /*

        protected override void OnNavigatedFrom(EventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            GraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);
            
            // TODO save Settings here

            base.OnNavigatedFrom(e);
        }
        */

        /*
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (GameController.I.State == GameState.StartScreen)
            {
                base.OnBackKeyPress(e);
            }
            else
            {
                e.Cancel = true;
                GameController.I.LoadStartScreen();
            }
        }
        */

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            GameTime gameTime = new GameTime(e.TotalTime, e.ElapsedTime);
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();
                switch (gesture.GestureType)
                {
                    case GestureType.Tap:
                        ButtonComponent bc = ButtonComponents.FirstOrDefault(
                            b => b.IsTap((int)gesture.Position.X, (int)gesture.Position.Y));
                        if (bc != null)
                        {
                            bc.Tap();
                        }
                        break;
                    default:
                        break;
                }
            }

            // update the level
            if (levelComponent != null)
            {
                levelComponent.Update(gameTime);
            }
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            GraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.Black);

            GameTime gameTime = new GameTime(e.TotalTime, e.ElapsedTime);
            foreach (GameComponent component in Components)
            {
                component.Draw(gameTime);
            }
            foreach (ButtonComponent component in ButtonComponents)
            {
                component.Draw(gameTime);
            }
        }

        // Internals ?

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
        }

   
    }
}

