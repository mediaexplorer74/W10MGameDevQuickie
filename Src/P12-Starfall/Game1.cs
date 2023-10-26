using FbonizziMonoGame.Assets;
using FbonizziMonoGame.Drawing;
using FbonizziMonoGame.Drawing.Abstractions;
using FbonizziMonoGame.Extensions;
using FbonizziMonoGame.Input;
using FbonizziMonoGame.Input.Abstractions;
using FbonizziMonoGame.PlatformAbstractions;
using FbonizziMonoGame.Sprites;
using FbonizziMonoGame.StringsLocalization;
using FbonizziMonoGame.UI.RateMe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameManager.About;
using GameManager.Assets;
using GameManager.Menu;
using System;
using System.Collections.Generic;
using System.Globalization;
using FbonizziMonoGame.Implementations;
namespace GameManager
{

    public class Game1 : Game, IFbonizziGame
    { 
        private const string GameName = "Starfall";

        private enum RunningStates
        {
            Splashscreen,
            Running
        }

        private readonly Uri _rateMeUri;
        private RunningStates _currentState;

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        private SpriteBatch _spriteBatch;

        private SplashScreenLoader _splashScreenLoader;

       
        private readonly ISettingsRepository _settingsRepository;
        private readonly IWebPageOpener _webPageOpener;
        private IEnumerable<IInputListener> _inputListeners;

        private readonly CultureInfo _gameCulture;

        public event EventHandler ExitGameRequested;
        private AssetsLoader _assetsLoader;

        private IScreenTransformationMatrixProvider _dynamicScaleMatrixProvider;

        private Sprite _mousePointer;

        private GameOrchestrator _orchestrator;
       
        private bool _isPc = true; // // TRUE - for TEST


        WindowsTextFileImporter _textFileAssetsLoader = new WindowsTextFileImporter();
      
        private readonly InMemoryLocalizedStringsRepository _localizedStringsRepository;


        // Just for MonoGame.Framework.WindowsUniversal bootstrapping requirement
        public Game1() 
        {
            //RnD
            //********************

            int? deviceWidth  = 640; //1280
            int? deviceHeight = 480; //800

            //RnD
            bool _isFullScreen = true;  //!!! "false" for Desktop: set it as "true" for W10M !!!              

            Window.Title = GameName;

            _isPc = true;
            _rateMeUri = default;//rateMeUri;
            _currentState = RunningStates.Splashscreen;

            //RnD             
            _settingsRepository = default;//settingsRepository;
            _webPageOpener = default;//webPageOpener;

            _gameCulture = new CultureInfo("en-US"); ;//gameCulture;

            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.LandscapeLeft
                | DisplayOrientation.Portrait,//| DisplayOrientation.LandscapeLeft,
                IsFullScreen = _isFullScreen
            };

            if (deviceWidth != null && deviceHeight != null)
            {
                GraphicsDeviceManager.PreferredBackBufferWidth = deviceWidth.Value;
                GraphicsDeviceManager.PreferredBackBufferHeight = deviceHeight.Value;
        
       
          }

            _localizedStringsRepository = new InMemoryLocalizedStringsRepository
            (
                new Dictionary<string, string>()
            );

            Content.RootDirectory = "Content";

            //RnD (Remark it to hide)
            //IsMouseVisible = true;
         
            //RnD
            _localizedStringsRepository = new InMemoryLocalizedStringsRepository(
                new Dictionary<string, string>());
        }

       
        protected override void Initialize()
        {
            _dynamicScaleMatrixProvider = new DynamicScalingMatrixProvider(
                new GameWindowScreenSizeChangedNotifier(Window),
                GraphicsDeviceManager.GraphicsDevice,
                800, 480,
                true);

            //RnD (Remark it to hide)
            //IsMouseVisible = false;

            base.Initialize();
        }

        private void LoadGameAssets()
        {
            new GameStringsLoader(_localizedStringsRepository, _gameCulture);

            //RnD : PLAN A
            AssetsLoader _assetsLoader = new AssetsLoader(Content, _textFileAssetsLoader);
            _assetsLoader.LoadResources();
            _mousePointer = _assetsLoader.Sprites["manina"];

            //RnD: PLAN B
            //_assetsLoader = new AssetsLoader(Content);
            //_assetsLoader.LoadResources();
            //_mousePointer = _assetsLoader.OtherSprites["manina"];
            //_soundManager = new SoundManager(_assetsLoader); //?


            var gameFactory = new Func<StarfallGame>(
                () => new StarfallGame(
                    _dynamicScaleMatrixProvider,
                    _assetsLoader,
                    _settingsRepository,
                    _orchestrator));

            var dialogDefinition = new Rectangle(
                _dynamicScaleMatrixProvider.VirtualWidth / 2 - 350, 24, 700,
                _dynamicScaleMatrixProvider.VirtualHeight - 60);

            RateMeDialog rateMeDialog = new RateMeDialog(
                launchesUntilPrompt: 2,
                maxRateShowTimes: 2,
                rateAppUri: _rateMeUri,
                dialogDefinition: dialogDefinition,
                font: _assetsLoader.Font,
                localizedStringsRepository: _localizedStringsRepository,
                rateMeDialogStrings: _gameCulture.TwoLetterISOLanguageName == "it"
                ? new DefaultItalianRateMeDialogStrings(GameName)
                : (RateMeDialogStrings)new DefaultEnglishRateMeDialogStrings(GameName),
                webPageOpener: _webPageOpener,
                //RnD
                settingsRepository: _settingsRepository,
                buttonADefinition: new Rectangle(
                    dialogDefinition.X + 150,
                    dialogDefinition.Y + 350,
                    140, 40),
                buttonBDefinition: new Rectangle(
                    dialogDefinition.X + dialogDefinition.Width - 140 - 150,
                    dialogDefinition.Y + 350,
                    140, 40),
                backgroundColor: Color.DarkGray.WithAlpha(1f),
                buttonsBackgroundColor: (new Color(255, 18, 67)).WithAlpha(1f),
                buttonsShadowColor: Color.Black,
                backgroundShadowColor: Color.Black.WithAlpha(1f),
                titleColor: Color.Black,
                buttonsTextColor: new Color(255, 234, 135),
                titlePositionOffset: new Vector2(dialogDefinition.Width / 2, 80f),
                buttonTextPadding: 40f,
                titlePadding: 160f);

            var menuFactory = new Func<MainMenuPage>(
                () => new MainMenuPage(
                    _assetsLoader,
                    rateMeDialog,
                    _settingsRepository,
                    _dynamicScaleMatrixProvider,
                    _localizedStringsRepository));

            var textsShowFactory = new Func<IncipitPage>(
                () => new IncipitPage(
                    _assetsLoader,
                    new List<string>()
                     {
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString1),
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString2),
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString3),
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString4),
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString5),
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString6),
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString7),
                          _localizedStringsRepository.Get(
                              GameStringsLoader.SlideshowTextString8)
                     }));

            var scoreFactory = new Func<ScorePage>(
                () => new ScorePage(
                    _assetsLoader,
                    _settingsRepository,
                    _dynamicScaleMatrixProvider,
                    _localizedStringsRepository));

            var protipPosition = new Vector2(50f, _dynamicScaleMatrixProvider.VirtualHeight
                - 50);

            const float protipTextScale = 0.4f;
            
            _orchestrator = new GameOrchestrator(
                gameFactory,
                menuFactory,
                textsShowFactory,
                scoreFactory,
                new ProtipsShower(
                    _assetsLoader.Font,
                    new List<Protip>()
                    {
                        new Protip()
                        {
                            Image = _assetsLoader.Sprites["TIP_1"],
                            Text = _localizedStringsRepository.Get(GameStringsLoader.ProTip1),
                            TextDrawingInfos = new DrawingInfos()
                            { Position = protipPosition,
                                Scale = protipTextScale,
                                Origin = new Vector2(0f, _assetsLoader.Font.GetTextCenter(
                                    _localizedStringsRepository.Get(
                                        GameStringsLoader.ProTip1)).Y)}
                        },
                        new Protip()
                        {
                            Image = _assetsLoader.Sprites["TIP_2"],
                            Text = _localizedStringsRepository.Get(GameStringsLoader.ProTip2),
                            TextDrawingInfos = new DrawingInfos() {
                                Position = protipPosition, Scale = protipTextScale,
                                Origin = new Vector2(0f,
                                _assetsLoader.Font.GetTextCenter(_localizedStringsRepository.Get(
                                    GameStringsLoader.ProTip2)).Y)}
                        },
                        new Protip()
                        {
                            Image = _assetsLoader.Sprites["TIP_3"], 
                            Text = _localizedStringsRepository.Get(GameStringsLoader.ProTip3),
                            TextDrawingInfos = new DrawingInfos() { 
                                Position = protipPosition, Scale = protipTextScale,
                                Origin = new Vector2(0f, _assetsLoader.Font.GetTextCenter(
                                    _localizedStringsRepository.Get(
                                        GameStringsLoader.ProTip3)).Y)}
                        },
                        new Protip()
                        {
                            Image = _assetsLoader.Sprites["TIP_4"], 
                            Text = _localizedStringsRepository.Get(GameStringsLoader.ProTip4),
                            TextDrawingInfos = new DrawingInfos() { 
                                Position = protipPosition, Scale = protipTextScale,
                                Origin = new Vector2(0f, _assetsLoader.Font.GetTextCenter(
                                    _localizedStringsRepository.Get(
                                        GameStringsLoader.ProTip4)).Y)}
                        },
                        new Protip()
                        {
                            Image = _assetsLoader.Sprites["TIPS_glow"], 
                            Text = _localizedStringsRepository.Get(
                                GameStringsLoader.ProTipGlow),
                            TextDrawingInfos = new DrawingInfos()
                            { Position = protipPosition, Scale = protipTextScale,
                                Origin = new Vector2(0f,
                                _assetsLoader.Font.GetTextCenter(_localizedStringsRepository.Get(
                                    GameStringsLoader.ProTipGlow)).Y) }
                        },
                        new Protip()
                        {
                            Image = _assetsLoader.Sprites["TIPS_life"], 
                            Text = _localizedStringsRepository.Get(GameStringsLoader.ProTipLife),
                            TextDrawingInfos = new DrawingInfos() {
                                Position = protipPosition + new Vector2(300, 0), 
                                Scale = protipTextScale,
                                Origin = new Vector2(0f, _assetsLoader.Font.GetTextCenter(
                                    _localizedStringsRepository.Get(
                                        GameStringsLoader.ProTipLife)).Y)}
                        },
                        new Protip()
                        {
                            Image = _assetsLoader.Sprites["TIPS_timejump"], 
                            Text = _localizedStringsRepository.Get(
                                GameStringsLoader.ProTipTimeJump),
                            TextDrawingInfos = new DrawingInfos() {
                                Position = protipPosition, Scale = protipTextScale,
                                Origin = new Vector2(
                                    0f, _assetsLoader.Font.GetTextCenter(
                                        _localizedStringsRepository.Get(
                                            GameStringsLoader.ProTipTimeJump)).Y)}
                        }
                    }),
                _spriteBatch.GraphicsDevice,
                _assetsLoader,
                _settingsRepository,
                _dynamicScaleMatrixProvider,
                _webPageOpener,
                _localizedStringsRepository);

            var mouseListener = new MouseListener(_dynamicScaleMatrixProvider);
            mouseListener.MouseDown += MouseListener_MouseClicked;

            var touchListener = new TouchListener(_dynamicScaleMatrixProvider);
            touchListener.TouchStarted += TouchListener_TouchEnded;

            var keyboardListener = new KeyboardListener();
            keyboardListener.KeyPressed += KeyboardListener_KeyPressed;

            var gamepadListener = new GamePadListener();
            gamepadListener.ButtonDown += GamepadListener_ButtonDown;

            _inputListeners = new List<IInputListener>()
            {
                mouseListener,
                touchListener,
                keyboardListener,
                gamepadListener
            };
        }//

        private void GamepadListener_ButtonDown(object sender, GamePadEventArgs e)
        {
            if (e.Button == Buttons.Back)
            {
                _orchestrator.Back();
                if (_orchestrator.ShouldEndApplication)
                {
                    if (ExitGameRequested != null)
                    {
                        ExitGameRequested(this, EventArgs.Empty); // Se ho un handler specifico, uso quello
                    }
                    else
                    {
                        Exit(); // Devono ancora fixare il problema dell'uscita da Android
                    }
                }
            }
        }

        private void KeyboardListener_KeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.P)
            {
                _orchestrator.TogglePause();
            }
            else if (e.Key == Keys.Escape)
            {
                _orchestrator.Back();
                if (_orchestrator.ShouldEndApplication)
                {
                    if (ExitGameRequested != null)
                    {
                        ExitGameRequested(this, EventArgs.Empty); // Se ho un handler specifico, uso quello
                    }
                    else
                    {
                        Exit(); // Devono ancora fixare il problema dell'uscita da Android
                    }
                }
            }
            else if (e.Key == Keys.Back) // L'indietro di Android viene triggerato qui!
            {
                _orchestrator.Back();
                if (_orchestrator.ShouldEndApplication)
                {
                    if (ExitGameRequested != null)
                    {
                        ExitGameRequested(this, EventArgs.Empty); // Se ho un handler specifico, uso quello
                    }
                    else
                    {
                        Exit(); // Devono ancora fixare il problema dell'uscita da Android
                    }
                }
            }
            else if (e.Key == Keys.Space)
            {
                _orchestrator.HandleInput();
            }
        }

        private void TouchListener_TouchEnded(object sender, TouchEventArgs e)
            => _orchestrator.HandleInput(e.Position);

        private void MouseListener_MouseClicked(object sender, MouseEventArgs e)
            => _orchestrator.HandleInput(e.Position);

        protected override void LoadContent()
        {
 Content.RootDirectory = "Content";
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _splashScreenLoader = new SplashScreenLoader(
                LoadGameAssets,
                Content,
                "Splashscreen");
            _splashScreenLoader.Load();

            _splashScreenLoader.Completed += SplashScreenLoader_Completed;
        }


        private void SplashScreenLoader_Completed(object sender, EventArgs e)
        {
            _splashScreenLoader = null;
            _orchestrator.Start();
        }

        public void Pause()
        {
            _orchestrator?.Pause();
        }

        public void Resume()
        {
            _orchestrator?.Resume();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            TimeSpan elapsed = gameTime.ElapsedGameTime;

            if (_splashScreenLoader != null)
            {
                _splashScreenLoader.Update(elapsed);
                return;
            }

            foreach (var listener in _inputListeners)
            {
                listener.Update(gameTime);
            }

            _orchestrator.Update(elapsed);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            //if (_currentState == RunningStates.Splashscreen)
            if (_splashScreenLoader != null)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin(transformMatrix: _dynamicScaleMatrixProvider.ScaleMatrix);
                _splashScreenLoader.Draw(_spriteBatch);
                _spriteBatch.End();
                return;
            }

            _orchestrator.Draw(_spriteBatch, GraphicsDevice);

            //if (_isPc)
            //{
                if (!_orchestrator.IsPaused)
                {
                    var mouseState = Mouse.GetState();
                    var mousePosition = new Vector2(mouseState.X - 32, mouseState.Y);
                    if (mouseState.X != 0 && mouseState.Y != 0)
                    {
                        _spriteBatch.Begin();
                        _spriteBatch.Draw(_mousePointer.Sheet, mousePosition, 
                            _mousePointer.SourceRectangle, Color.White);
                        _spriteBatch.End();
                    }
                }
            //}

            base.Draw(gameTime);
        }

    }
}

