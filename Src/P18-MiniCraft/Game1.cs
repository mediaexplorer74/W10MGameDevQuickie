// GameManager.Game1

using dpLogo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using GameManager;
using GameManager.screen;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using GameManager.entity;
using Microsoft.Xna.Framework.Content;
using GameManager.gfx;
using GameManager.level;
using Color = Microsoft.Xna.Framework.Color;
using GameManager.item;
using GameManager.level.tile;
using GameManager.sound;
//using System.Threading;

#nullable disable
namespace GameManager
{
  public class Game1 : Game
  {
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameManager _gameManager;
    private SpriteFont _spriteFont;

    public const string NAME = "Minicraft";
    public const int LevelCount = 5;

    public int HEIGHT = 800;
    public int WIDTH = 600;

    public Game1.GetSaveDateDelegate OnGetSaveDate;
    public Game1.SaveGameDelegate OnSaveGame;
    public Game1.LoadGameDelegate OnLoadGame;
    public Game1.ClearInputDelegate OnClearInput;
    public Game1.AutosaveDelegate OnAutosave;
      
    public int GameTime;
    public bool HasWon;
    public Player ActivePlayer;
    public AirWizard ActiveAirWizard;
    public Menu ActiveMenu;
    private ContentManager content;
    private GraphicsDevice graphicsDevice;
    private Texture2D myRenderTarget1;
    private Texture2D myRenderTarget2;
    private bool buffer1Active = true;
    private Microsoft.Xna.Framework.Color[] destinationColors;
    private Screen screen;
    private Screen lightScreen;
    private Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[256];
    private int tickCount;
    private Level level;
    private Level[] levels = new Level[5];
    private int currentLevel = 3;
    private int playerDeadTime;
    private int pendingLevelChange;
    private int wonTimer;


    private const string filename = "mcsave";
    private const short fileversion = 1;
    private const int destWidth = 800;
    private const int destHeight = 480;
    private const string versionStr = "1.4";
    private const int miniMapWidth = 80;
    private const int miniMapHeight = 50;
    
    //private GraphicsDeviceManager myGraphics;
    //private SpriteBatch mySpriteBatch;
    //private SpriteFont mySpriteFont;

    private Texture2D imgDown;
    private Texture2D imgLeft;
    private Texture2D imgPause;
    private Texture2D imgDisc;
    private Vector2 scorePos = new Vector2(52f, 0.0f);
    private Texture2D miniMap;
    private Color[] miniMapData;
    private Rectangle miniMapRect;
    private Color miniMapCol;
    private Dictionary<MiniMapTile, Color> miniMapColors;
    private RenderTarget2D dpLogoTarget1;
    private RenderTarget2D dpLogoTarget2;
    private bool dpLogoActive1;
    private dpLogoDisplayer myLogo;
    private float factor = 0.05f;
    private Game1 myGame;
    private KeyboardState oldKeys;
    private KeyboardState newKeys;
    private bool keyPressed;
    private GamePadState oldPad;
    private GamePadState newPad;
    private TouchCollection newTouch;
    private TouchCollection oldTouch;
    private Rectangle fRectUp;
    private Rectangle fRectDown;
    private Rectangle fRectLeft;
    private Rectangle fRectRight;
    private Rectangle fRectUpImg;
    private Rectangle fRectDownImg;
    private Rectangle fRectLeftImg;
    private Rectangle fRectRightImg;
    private int fUpId = int.MinValue;
    private int fDownId = int.MinValue;
    private int fLeftId = int.MinValue;
    private int fRightId = int.MinValue;
    private int fAttackID = int.MinValue;
    private int fMenuID = int.MinValue;
    private Rectangle fRectAttack;
    private Rectangle fRectMenu;
    private Color normColKey;
    private Color pressedColKey;
    private Color normColArrow;
    private Color pressedColArrow;
    private Rectangle fPauseRect;
    private Rectangle fDiscRect1;
    private Rectangle fDiscRect2;
    private Rectangle fPauseRectImg;
    private Rectangle fDiscRectImg1;
    private Rectangle fDiscRectImg2;
    private TimeSpan attackKeyTime;
    private int attackCount;
    private string overlayText;
    private int overlayTimer;
    private Game1.StartStatus myStatus;
    private bool initFinished;
    private int doQuicksave;
    private int doAutosave;
    public InputHandler Input;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        //graphics.SynchronizeWithVerticalRetrace = false;
        Content.RootDirectory = "Content";

        // Frame rate is 30 fps by default for Windows Phone.
        TargetElapsedTime = TimeSpan.FromTicks(333333);

            //Create a new instance of the Screen Manager

            Glob.Content = Content;
            //_gameManager = new GameManager(_graphics);

            //Components.Add(_gameManager);

            IsMouseVisible = true;
            /*
      this._graphics = new GraphicsDeviceManager(this);
      this.Content.RootDirectory = "Content";

      IsMouseVisible = true;

      this.TargetElapsedTime = TimeSpan.FromTicks(166666L);
      
      this.InactiveSleepTime = TimeSpan.FromSeconds(1.0);
      
      this._graphics.PreferredBackBufferWidth = 800;
      this._graphics.PreferredBackBufferHeight = 480;
      
       this._graphics.IsFullScreen = false;
            */

            //RnD
            //PhoneApplicationService.Current.Deactivated += 
            //          new EventHandler<DeactivatedEventArgs>(this.GameDeactivated);
            //PhoneApplicationService.Current.Activated += 
            //          new EventHandler<ActivatedEventArgs>(this.GameActivated);

            this.myLogo = new dpLogoDisplayer((Microsoft.Xna.Framework.Game) this, this._graphics);

            //RnD
            this.myStatus = Game1.StartStatus.readyForGame;//.logoStart;
        }

    protected override void Initialize()
    {
       
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;

        _graphics.ApplyChanges();


      this.dpLogoTarget1 = new RenderTarget2D(this.GraphicsDevice,
      this._graphics.PreferredBackBufferWidth, this._graphics.PreferredBackBufferHeight);

      this.dpLogoTarget2 = new RenderTarget2D(this.GraphicsDevice, 
          this._graphics.PreferredBackBufferWidth, this._graphics.PreferredBackBufferHeight);
    
      this.dpLogoActive1 = false;
      this.miniMap = new Texture2D(this.GraphicsDevice, 80, 50);
      this.miniMapData = new Color[4000];

      this.miniMapRect = new Rectangle(
          this._graphics.PreferredBackBufferWidth - this.miniMap.Width * 3 / 2,
          0, this.miniMap.Width * 3 / 2, this.miniMap.Height * 3 / 2);

      this.miniMapCol = Color.White * 0.7f;
      this.miniMapColors = new Dictionary<MiniMapTile, Color>();

      this.miniMapColors.Add(MiniMapTile.notOnMap, Color.Black);
      this.miniMapColors.Add(MiniMapTile.unknown, Color.Black);
      this.miniMapColors.Add(MiniMapTile.Cactus, Color.LawnGreen);
      this.miniMapColors.Add(MiniMapTile.CactusSapling, Color.LawnGreen);
      this.miniMapColors.Add(MiniMapTile.Cloud, Color.WhiteSmoke);
      this.miniMapColors.Add(MiniMapTile.CloudCactus, Color.LightGreen);
      this.miniMapColors.Add(MiniMapTile.Dirt, Color.Brown);
      this.miniMapColors.Add(MiniMapTile.Farmland, Color.Brown);
      this.miniMapColors.Add(MiniMapTile.Flower, Color.LawnGreen);
      this.miniMapColors.Add(MiniMapTile.GemOre, Color.LightSteelBlue);
      this.miniMapColors.Add(MiniMapTile.GoldOre, Color.Gold);
      this.miniMapColors.Add(MiniMapTile.Grass, Color.LawnGreen);
      this.miniMapColors.Add(MiniMapTile.HardRock, Color.Gray);
      this.miniMapColors.Add(MiniMapTile.Hole, Color.Brown);
      this.miniMapColors.Add(MiniMapTile.InfiniteFall, Color.CornflowerBlue);
      this.miniMapColors.Add(MiniMapTile.IronOre, Color.DarkSlateGray);
      this.miniMapColors.Add(MiniMapTile.Lava, Color.OrangeRed);
      this.miniMapColors.Add(MiniMapTile.Rock, Color.White);
      this.miniMapColors.Add(MiniMapTile.Sand, Color.Yellow);
      this.miniMapColors.Add(MiniMapTile.StairsDown, Color.Red);
      this.miniMapColors.Add(MiniMapTile.StairsUp, Color.Red);
      this.miniMapColors.Add(MiniMapTile.Tree, Color.ForestGreen);
      this.miniMapColors.Add(MiniMapTile.TreeSapling, Color.LawnGreen);
      this.miniMapColors.Add(MiniMapTile.Water, Color.Blue);
      this.miniMapColors.Add(MiniMapTile.Wheat, Color.GreenYellow);

      this.fPauseRect = new Rectangle(0, 0, 48, 48);
      this.fDiscRect1 = new Rectangle(752, 0, 48, 48);
      this.fDiscRect2 = new Rectangle(752, this.miniMapRect.Bottom, 48, 48);
      this.fPauseRectImg = new Rectangle(8, 8, 32, 32);
      this.fDiscRectImg1 = new Rectangle(760, 8, 32, 32);
      this.fDiscRectImg2 = new Rectangle(760, 8 + this.miniMapRect.Bottom, 32, 32);
      this.normColKey = Color.White * 0.5f;
      this.pressedColKey = Color.Red * 0.5f;
      this.normColArrow = Color.White * 0.5f;
      this.pressedColArrow = Color.Red * 0.5f;
      this.myLogo.Initialize();
            

      Glob.Content = Content;

      //RnD
      _gameManager = new GameManager(default);

      base.Initialize();
    }

    protected override void LoadContent()
    {
      this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.myLogo.SpriteBatch = this._spriteBatch;
      //this._spriteFont = this.Content.Load<SpriteFont>("miniFont");
      this.myLogo.LoadTheContent();
      miniprefs.Load();

      //RnD
      //new Thread(new ThreadStart(this.initGame)).Start();
      this.initGame();
    }

    private void initGame()
    {
      this.fRectAttack = new Rectangle(650, 300, 150, 80);
      this.fRectMenu = new Rectangle(650, 400, 150, 80);
      this.myGame = new Game1();
      this.myGame.OnGetSaveDate = new Game1.GetSaveDateDelegate(this.GetSaveDate);
      this.myGame.OnLoadGame = new Game1.LoadGameDelegate(this.LoadGame);
      this.myGame.OnSaveGame = new Game1.SaveGameDelegate(this.SaveGame);
      this.myGame.OnClearInput = new Game1.ClearInputDelegate(this.ClearInput);
      this.myGame.OnAutosave = new Game1.AutosaveDelegate(this.PrepareAutosave);
      this.myGame.Init(this.Content, this.GraphicsDevice);
            
      //RnD
      //if (PhoneApplicationService.Current.StartupMode == null)
        this.LoadGame(9);

      this.attackKeyTime = TimeSpan.FromMilliseconds(0.0);
      this.imgDown = this.Content.Load<Texture2D>("down");
      this.imgLeft = this.Content.Load<Texture2D>("left");
      this.fRectUp = new Rectangle(0, 239, 240, 120);
      this.fRectDown = new Rectangle(0, 360, 240, 120);
      this.fRectLeft = new Rectangle(0, 240, 120, 240);
      this.fRectRight = new Rectangle(120, 240, 120, 240);
      this.fRectUpImg.X = this.fRectUp.X + (this.fRectUp.Width - this.imgDown.Width) / 2;
      this.fRectUpImg.Y = this.fRectUp.Y + (this.fRectUp.Height - this.imgDown.Height) / 2;
      this.fRectUpImg.Width = this.imgDown.Width;
      this.fRectUpImg.Height = this.imgDown.Height;
      this.fRectDownImg.X = this.fRectDown.X + (this.fRectDown.Width - this.imgDown.Width) / 2;
      this.fRectDownImg.Y = this.fRectDown.Y + (this.fRectDown.Height - this.imgDown.Height) / 2;
      this.fRectDownImg.Width = this.imgDown.Width;
      this.fRectDownImg.Height = this.imgDown.Height;
      this.fRectLeftImg.X = this.fRectLeft.X + (this.fRectLeft.Width - this.imgLeft.Width) / 2;
      this.fRectLeftImg.Y = this.fRectLeft.Y + (this.fRectLeft.Height - this.imgLeft.Height) / 2;
      this.fRectLeftImg.Width = this.imgLeft.Width;
      this.fRectLeftImg.Height = this.imgLeft.Height;
      this.fRectRightImg.X = this.fRectRight.X + (this.fRectRight.Width - this.imgLeft.Width) / 2;
      this.fRectRightImg.Y = this.fRectRight.Y + (this.fRectRight.Height - this.imgLeft.Height) / 2;
      this.fRectRightImg.Width = this.imgLeft.Width;
      this.fRectRightImg.Height = this.imgLeft.Height;
      this.imgPause = this.Content.Load<Texture2D>("pause");
      this.imgDisc = this.Content.Load<Texture2D>("disc");
      this.initFinished = true;
    }

    protected override void UnloadContent()
    {
    }

    protected override void Update(GameTime gameTime)
    {
      if (this.doQuicksave == 2)
      {
        this.doQuicksave = 0;
        this.SaveGame(0);
      }
      else if (this.doAutosave == 2)
      {
        this.doAutosave = 0;
        this.SaveGame(4);
      }

      this.oldKeys = this.newKeys;
      this.newKeys = Keyboard.GetState();
      this.oldPad = this.newPad;
      this.newPad = GamePad.GetState(PlayerIndex.One);
      this.oldTouch = this.newTouch;
      this.newTouch = TouchPanel.GetState();
      
      if (this.myLogo != null)
      {
        this.myLogo.Update(gameTime);
        if (this.myLogo.Finished && this.initFinished)
          this.myStatus = Game1.StartStatus.readyForGame;
      }

      if (this.myStatus == Game1.StartStatus.game && this.myGame != null)
      {
        this.checkKey(Keys.Up, InputHandler.KeyType.up);
        this.checkKey(Keys.Down, InputHandler.KeyType.down);
        this.checkKey(Keys.Left, InputHandler.KeyType.left);
        this.checkKey(Keys.Right, InputHandler.KeyType.right);
        this.checkKey(Keys.X, InputHandler.KeyType.menu);
        this.checkAttackKey(Keys.C, gameTime.TotalGameTime);

        if (this.oldKeys.IsKeyUp(Keys.Escape) && this.newKeys.IsKeyDown(Keys.Escape))
        {
            if (this.myGame.EscapeClicked())
                this.Quit();
        }
        else if (this.oldKeys.IsKeyUp(Keys.F5) && this.newKeys.IsKeyDown(Keys.F5))
        {
            this.PrepareQuicksave();
        }
        else if (this.oldKeys.IsKeyUp(Keys.F9) && this.newKeys.IsKeyDown(Keys.F9))
        {
            this.LoadGame(0);
        }

        if (this.oldPad.Buttons.Back == ButtonState.Released
                    && this.newPad.Buttons.Back == ButtonState.Pressed
                    && this.myGame.EscapeClicked())
        {
            this.Quit();
        }


        for (int index = 0; index < this.newTouch.Count; ++index)
        {
          if (this.newTouch[index].State == TouchLocationState.Pressed)
          {
            this.CheckUp(this.newTouch[index]);
            this.CheckDown(this.newTouch[index]);
            this.CheckLeft(this.newTouch[index]);
            this.CheckRight(this.newTouch[index]);

            if (this.fRectAttack.Contains((int) this.newTouch[index].Position.X,
                (int) this.newTouch[index].Position.Y))
              this.fAttackID = this.newTouch[index].Id;
            if (this.fRectMenu.Contains((int) this.newTouch[index].Position.X, 
                (int) this.newTouch[index].Position.Y))
            {
              this.fMenuID = this.newTouch[index].Id;
              
              //RnD
              this.myGame.Input.KeyPressed(InputHandler.KeyType.menu);
            }
            if (ActiveMenu == null)
            {
              if (miniprefs.Minimap)
              {
                if (this.fDiscRect2.Contains((int) this.newTouch[index].Position.X,
                    (int) this.newTouch[index].Position.Y))
                  this.PrepareQuicksave();
              }
              else if (this.fDiscRect1.Contains((int) this.newTouch[index].Position.X,
                  (int) this.newTouch[index].Position.Y))
                this.PrepareQuicksave();

              if (this.fPauseRect.Contains((int) this.newTouch[index].Position.X,
                  (int) this.newTouch[index].Position.Y))
                this.myGame.SetMenu((Menu) new PauseMenu(this.myGame));
            }
          }
          else if (this.newTouch[index].State == TouchLocationState.Moved)
          {
            if (this.newTouch[index].Id == this.fUpId && !this.HitUp(this.newTouch[index]))
            {
              InputHandler.KeyReleased(InputHandler.KeyType.up);

              this.CheckDown(this.newTouch[index]);
              this.CheckLeft(this.newTouch[index]);
              this.CheckRight(this.newTouch[index]);
            }
            if (this.newTouch[index].Id == this.fDownId && !this.HitDown(this.newTouch[index]))
            {
              InputHandler.KeyReleased(InputHandler.KeyType.down);
              this.CheckUp(this.newTouch[index]);
              this.CheckLeft(this.newTouch[index]);
              this.CheckRight(this.newTouch[index]);
            }
            if (this.newTouch[index].Id == this.fLeftId && !this.HitLeft(this.newTouch[index]))
            {
              InputHandler.KeyReleased(InputHandler.KeyType.left);
              this.CheckUp(this.newTouch[index]);
              this.CheckDown(this.newTouch[index]);
              this.CheckRight(this.newTouch[index]);
            }
            if (this.newTouch[index].Id == this.fRightId && !this.HitRight(this.newTouch[index]))
            {
              InputHandler.KeyReleased(InputHandler.KeyType.right);
              this.CheckUp(this.newTouch[index]);
              this.CheckDown(this.newTouch[index]);
              this.CheckLeft(this.newTouch[index]);
            }
          }
          else if (this.newTouch[index].State == TouchLocationState.Released)
          {
            if (this.newTouch[index].Id == this.fUpId)
               InputHandler.KeyReleased(InputHandler.KeyType.up);

            if (this.newTouch[index].Id == this.fDownId)
               InputHandler.KeyReleased(InputHandler.KeyType.down);

            if (this.newTouch[index].Id == this.fLeftId)
               InputHandler.KeyReleased(InputHandler.KeyType.left);

            if (this.newTouch[index].Id == this.fRightId)
               InputHandler.KeyReleased(InputHandler.KeyType.right);

            if (this.newTouch[index].Id == this.fMenuID)
               InputHandler.KeyReleased(InputHandler.KeyType.menu);

            if (this.newTouch[index].Id == this.fAttackID)
            {
              InputHandler.KeyReleased(InputHandler.KeyType.attack);
              this.attackCount = 0;
              this.fAttackID = 0;
            }
          }
          if (this.fAttackID != 0)
            this.AttackPressed(gameTime.TotalGameTime);
        }
        //RnD
        this.myGame.Tick();
      }

        Glob.Update(gameTime);
            
        _gameManager.Update();

        base.Update(gameTime);
    }//Update

    public void AttackPressed(TimeSpan totalGameTime)
    {
      int num = this.attackCount != 1 ? 60 : 400;
      if ((totalGameTime - this.attackKeyTime).Milliseconds < num)
        return;
      //RnD
      //this.InputHandler.KeyPressed(InputHandler.KeyType.attack);
      
      this.attackKeyTime = totalGameTime;
      ++this.attackCount;
    }

    private void checkKey(Keys key, InputHandler.KeyType miniKey)
    {
      if (this.oldKeys.IsKeyUp(key) && this.newKeys.IsKeyDown(key))
      {
        //RnD
        this.myGame.Input.KeyPressed(miniKey);
      }
      else
      {
        if (!this.oldKeys.IsKeyDown(key) || !this.newKeys.IsKeyUp(key))
          return;
        this.CheckKeyColor();

        InputHandler.KeyReleased(miniKey);
      }
    }

    private void checkAttackKey(Keys key, TimeSpan totalGameTime)
    {
      if (this.newKeys.IsKeyDown(key))
      {
        this.AttackPressed(totalGameTime);
      }
      else
      {
        if (!this.oldKeys.IsKeyDown(key) || !this.newKeys.IsKeyUp(key))
          return;
        this.CheckKeyColor();

        InputHandler.KeyReleased(InputHandler.KeyType.menu);
        this.attackCount = 0;
      }
    }

    private void CheckKeyColor()
    {
      if (this.keyPressed)
        return;
      this.keyPressed = true;
      this.normColArrow = Color.White * 0.2f;
      this.pressedColKey = Color.Red * 0.1f;
    }

    private bool HitUp(TouchLocation tl)
    {
      if (!this.fRectUp.Contains((int) tl.Position.X, (int) tl.Position.Y))
        return false;
      int num1 = (int) tl.Position.X - this.fRectUp.X;
      int num2 = (int) tl.Position.Y - this.fRectUp.Y;
      return num1 > num2 && num1 < this.fRectUp.Width - num2;
    }

    private bool HitDown(TouchLocation tl)
    {
      if (!this.fRectDown.Contains((int) tl.Position.X, (int) tl.Position.Y))
        return false;
      int num1 = (int) tl.Position.X - this.fRectDown.X;
      int num2 = this.fRectDown.Height - ((int) tl.Position.Y - this.fRectDown.Y);
      return num1 > num2 && num1 < this.fRectDown.Width - num2;
    }

    private bool HitLeft(TouchLocation tl)
    {
      if (!this.fRectLeft.Contains((int) tl.Position.X, (int) tl.Position.Y))
        return false;
      int num1 = (int) tl.Position.X - this.fRectLeft.X;
      int num2 = (int) tl.Position.Y - this.fRectLeft.Y;
      return num2 > num1 && num2 < this.fRectLeft.Height - num1;
    }

    private bool HitRight(TouchLocation tl)
    {
      if (!this.fRectRight.Contains((int) tl.Position.X, (int) tl.Position.Y))
        return false;
      int num1 = this.fRectRight.Width - ((int) tl.Position.X - this.fRectRight.X);
      int num2 = (int) tl.Position.Y - this.fRectRight.Y;
      return num2 > num1 && num2 < this.fRectRight.Height - num1;
    }

    private void CheckUp(TouchLocation tl)
    {
      if (!this.HitUp(tl))
        return;
      this.fUpId = tl.Id;
      
      //RnD
      this.myGame.Input.KeyPressed(InputHandler.KeyType.up);
    }

    private void CheckDown(TouchLocation tl)
    {
      if (!this.HitDown(tl))
        return;
      this.fDownId = tl.Id;

      //RnD
      this.myGame.Input.KeyPressed(InputHandler.KeyType.down);
    }

    private void CheckLeft(TouchLocation tl)
    {
      if (!this.HitLeft(tl))
        return;
      this.fLeftId = tl.Id;

      //RnD
      this.myGame.Input.KeyPressed(InputHandler.KeyType.left);
    }

    private void CheckRight(TouchLocation tl)
    {
      if (!this.HitRight(tl))
        return;
      this.fRightId = tl.Id;
            
      //RnD
      this.myGame.Input.KeyPressed(InputHandler.KeyType.right);
    }

    protected override void Draw(GameTime gameTime)
    {
      // 1
      if (this.myLogo != null)
      {
        if (this.dpLogoActive1)
        {
          this.GraphicsDevice.SetRenderTarget(this.dpLogoTarget2);
          this.dpLogoActive1 = false;
        }
        else
        {
          this.GraphicsDevice.SetRenderTarget(this.dpLogoTarget1);
          this.dpLogoActive1 = true;
        }
        this.myLogo.Draw(gameTime);
        this.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);

        this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, 
            DepthStencilState.Default, RasterizerState.CullNone);

        if (this.dpLogoActive1)
          this._spriteBatch.Draw((Texture2D) this.dpLogoTarget1, new Rectangle(0, 0, 800, 480), 
              Color.White);
        else
          this._spriteBatch.Draw((Texture2D) this.dpLogoTarget2, new Rectangle(0, 0, 800, 480), 
              Color.White);

        //RnD
        _gameManager.Draw();

        this._spriteBatch.End();
      }

      // 2
      if (this.myStatus == Game1.StartStatus.game && this.myGame != null)
      {
        this.myGame.Render();
        this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp,
            DepthStencilState.Default, RasterizerState.CullNone);
        this._spriteBatch.Draw(this.myGame.Texture, new Rectangle(0, 0, 800, 480), Color.White);

        //RnD
        _gameManager.Draw();

        this._spriteBatch.End();
        this.DrawUI();
      }
      else if (this.myLogo != null && this.myLogo.Finished && this.initFinished &&
                (this.myStatus == Game1.StartStatus.logoStart
                || this.myStatus == Game1.StartStatus.readyForGame))
      {
        //3 

        Color color = Color.White * this.factor;
        this.myGame.Render();
        this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
            SamplerState.PointClamp,
            DepthStencilState.Default, RasterizerState.CullNone);

        this._spriteBatch.Draw(this.myGame.Texture, new Rectangle(0, 0, 800, 480), color);
        
        //RnD
        _gameManager.Draw();
        
        this._spriteBatch.End();

        this.factor += 0.05f;

        if ((double) this.factor >= 0.99000000953674316)
        {
          this.myStatus = Game1.StartStatus.game;
          this.Components.Remove((IGameComponent) this.myLogo);
          this.myLogo = (dpLogoDisplayer) null;
        }
      }

      base.Draw(gameTime);
    }//Draw

    public void DrawUI()
    {
      GameStatus gameStatus = GetGameStatus();
      string text1;
      string text2;
      switch (gameStatus)
      {
        case GameStatus.Menu_Select:
          text1 = string.Empty;
          text2 = "select";
          break;
        case GameStatus.Menu_Back:
          text1 = "select";
          text2 = "back";
          break;
        case GameStatus.Menu_BackOnly:
          text1 = string.Empty;
          text2 = "back";
          break;
        case GameStatus.Menu_Inventory:
          text1 = "select";
          text2 = "back";
          break;
        case GameStatus.Menu_Crafting:
          text1 = "craft";
          text2 = "back";
          break;
        case GameStatus.Menu_Chest:
          text1 = "select";
          text2 = "back";
          break;
        case GameStatus.Game_Attack:
          text1 = "attack";
          text2 = "inventory";
          break;
        case GameStatus.Game_Glove:
          text1 = "pick up";
          text2 = "inventory";
          break;
        case GameStatus.Game_Axe:
          text1 = "hack";
          text2 = "inventory";
          break;
        case GameStatus.Game_Hoe:
          text1 = "hoe";
          text2 = "inventory";
          break;
        case GameStatus.Game_Pickaxe:
          text1 = "pickaxe";
          text2 = "inventory";
          break;
        case GameStatus.Game_Shovel:
          text1 = "dig";
          text2 = "inventory";
          break;
        case GameStatus.Game_Use:
          text1 = "attack";
          text2 = "use";
          break;
        case GameStatus.Game_UseGlove:
          text1 = "pick up";
          text2 = "use";
          break;
        case GameStatus.Game_UseResource:
          text1 = "use";
          text2 = "inventory";
          break;
        case GameStatus.Game_Place:
          text1 = "place";
          text2 = "inventory";
          break;
        default:
          text1 = string.Empty;
          text2 = string.Empty;
          break;
      }
      if (this.keyPressed)
      {
        if (!string.IsNullOrEmpty(text1))
          text1 += " C";
        if (!string.IsNullOrEmpty(text2))
          text2 += " X";
      }
      this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
      if (text1.Length > 0)
      {
        Vector2 vector2 = this._spriteFont.MeasureString(text1);
        if (InputHandler.Attack.Pressed && InputHandler.Attack.Clicked)
          this._spriteBatch.DrawString(this._spriteFont, text1, 
              new Vector2((float) (this.fRectAttack.Right - (int) vector2.X - 8),
              (float) (this.fRectAttack.Top + this.fRectAttack.Height / 2 - (int) vector2.Y / 2)),
              this.pressedColKey);
        else
          this._spriteBatch.DrawString(this._spriteFont, text1, 
              new Vector2((float) (this.fRectAttack.Right - (int) vector2.X - 8), 
              (float) (this.fRectAttack.Top + this.fRectAttack.Height / 2 - (int) vector2.Y / 2)),
              this.normColKey);
      }

      if (text2.Length > 0)
      {
        Vector2 vector2 = this._spriteFont.MeasureString(text2);

        if (InputHandler.Menu.Pressed)
          this._spriteBatch.DrawString(this._spriteFont, text2, 
              new Vector2((float) (this.fRectMenu.Right - (int) vector2.X - 8), 
              (float) (this.fRectMenu.Top + this.fRectMenu.Height / 2 - (int) vector2.Y / 2)),
              this.pressedColKey);
        else
          this._spriteBatch.DrawString(this._spriteFont, text2,
              new Vector2((float) (this.fRectMenu.Right - (int) vector2.X - 8), 
              (float) (this.fRectMenu.Top + this.fRectMenu.Height / 2 - (int) vector2.Y / 2)), 
              this.normColKey);
      }
      if (gameStatus != GameStatus.Menu_BackOnly)
      {
        if (InputHandler.Up.Pressed)
          this._spriteBatch.Draw(this.imgDown, this.fRectUpImg, new Rectangle?(),
              this.pressedColArrow, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
        else
          this._spriteBatch.Draw(this.imgDown, this.fRectUpImg, new Rectangle?(), 
              this.normColArrow, 0.0f, Vector2.Zero, SpriteEffects.FlipVertically, 0.0f);
        if (InputHandler.Down.Pressed)
          this._spriteBatch.Draw(this.imgDown, this.fRectDownImg, new Rectangle?(), 
              this.pressedColArrow, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        else
          this._spriteBatch.Draw(this.imgDown, this.fRectDownImg, new Rectangle?(), 
              this.normColArrow, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        if (InputHandler.Left.Pressed)
          this._spriteBatch.Draw(this.imgLeft, this.fRectLeftImg, new Rectangle?(), 
              this.pressedColArrow, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        else
          this._spriteBatch.Draw(this.imgLeft, this.fRectLeftImg, new Rectangle?(), 
              this.normColArrow, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
        if (InputHandler.Right.Pressed)
          this._spriteBatch.Draw(this.imgLeft, this.fRectRightImg, new Rectangle?(), 
              this.pressedColArrow, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
        else
          this._spriteBatch.Draw(this.imgLeft, this.fRectRightImg, new Rectangle?(),
              this.normColArrow, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
      }
      if 
      (
        gameStatus == GameStatus.Game_Attack 
        || gameStatus == GameStatus.Game_Place
        || gameStatus == GameStatus.Game_Use 
        || gameStatus == GameStatus.Game_Axe
        || gameStatus == GameStatus.Game_Glove 
        || gameStatus == GameStatus.Game_Hoe
        || gameStatus == GameStatus.Game_Pickaxe 
        || gameStatus == GameStatus.Game_Shovel
      )
      {
        this._spriteBatch.Draw(this.imgPause, this.fPauseRectImg, this.normColKey);
        if (miniprefs.Minimap)
          this._spriteBatch.Draw(this.imgDisc, this.fDiscRectImg2, this.normColKey);
        else
          this._spriteBatch.Draw(this.imgDisc, this.fDiscRectImg1, this.normColKey);
      }

      if 
      (
        gameStatus == GameStatus.Game_Attack
        || gameStatus == GameStatus.Game_Place
        || gameStatus == GameStatus.Game_Use
        || gameStatus == GameStatus.Game_Axe
        || gameStatus == GameStatus.Game_Glove
        || gameStatus == GameStatus.Game_Hoe
        || gameStatus == GameStatus.Game_Pickaxe
        || gameStatus == GameStatus.Game_Shovel
        || this.myGame.ActiveMenu is PauseMenu
      )
      {
        this._spriteBatch.DrawString(this._spriteFont, "Score:"
        + this.myGame.ActivePlayer.Score.ToString((IFormatProvider)CultureInfo.CurrentCulture),
        this.scorePos, this.normColKey);
      }

      if (this.overlayTimer > 0)
      {
        --this.overlayTimer;
        float num = (float) (0.699999988079071 - (double) (200 - this.overlayTimer) / 200.0);
        if ((double) num > 0.0)
        {
          Color color = Color.White * num;
          Vector2 vector2 = this._spriteFont.MeasureString(this.overlayText);
          this._spriteBatch.DrawString(this._spriteFont, this.overlayText,
              new Vector2((float) (400.0 - (double) vector2.X / 2.0),
              (float) (240.0 - (double) vector2.Y / 2.0)), color);
        }
      }
      if (this.myGame.ActiveMenu == null && miniprefs.Minimap)
      {
        this.PrepareMiniMap();
        this._spriteBatch.Draw(this.miniMap, this.miniMapRect, this.miniMapCol);
      }
      if (this.myGame.ActiveMenu is TitleMenu)
        this._spriteBatch.DrawString(this._spriteFont, "Version 1.4",
            new Vector2(8f, 0.0f), this.normColKey);
      this._spriteBatch.End();
      if (this.doQuicksave == 1)
      {
        this.doQuicksave = 2;
      }
      else
      {
        if (this.doAutosave != 1)
          return;
        this.doAutosave = 2;
      }
    }

    private void PrepareMiniMap()
    {
      int index = 0;
      for (int y = this.myGame.ActivePlayer.Y / 16 - 25; y < this.myGame.ActivePlayer.Y / 16 + 25; ++y)
      {
        for (int x = this.myGame.ActivePlayer.X / 16 - 40; x < this.myGame.ActivePlayer.X / 16 + 40; ++x)
        {
          this.miniMapData[index] = this.miniMapColors[this.myGame.GetMiniMap(x, y)];
          ++index;
        }
      }
      this.miniMap.SetData<Color>(this.miniMapData);
    }

    public void PrepareQuicksave()
    {
      this.overlayText = "saving...    ";
      this.doQuicksave = 1;
      this.overlayTimer = 200;
    }

    public void PrepareAutosave()
    {
      this.overlayText = "autosaving...    ";
      this.doAutosave = 1;
      this.overlayTimer = 200;
    }

    public void SaveGame(int index)
    {
      if (this.myGame.ActiveMenu is LoadSaveMenu)
        this.myGame.SetMenu((Menu) null);
      try
      {
        using (IsolatedStorageFile storeForApplication 
                    = IsolatedStorageFile.GetUserStoreForApplication())
        {
          using (IsolatedStorageFileStream output = 
                        new IsolatedStorageFileStream("mcsave" 
                        + index.ToString((IFormatProvider) CultureInfo.InvariantCulture),
                        FileMode.Create, storeForApplication))
          {
            using (BinaryWriter writer = new BinaryWriter((Stream) output))
            {
            switch (index)
            {
                case 4:
                    this.overlayText = "autosaving...done";
                    break;
                case 9:

                    //RnD
                    //label_9:
                    do 
                    {
                        writer.Write((short)1);
                        writer.Write(DateTime.Now.ToString(
                            (IFormatProvider)CultureInfo.InvariantCulture));
                        this.myGame.SaveToWriter(writer);
                        //return;                     

                       this.overlayTimer = 200;
                       return;
                    } while (true);
                    // goto label_9;

                    default:
                        this.overlayText = "saving...done";
                        break;
                }
            }
          }
        }
      }
      catch
      {
        this.overlayText = "save error...";
        this.overlayTimer = 200;
      }
    }

    public void LoadGame(int index)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          using (IsolatedStorageFileStream input = new IsolatedStorageFileStream("mcsave" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture), FileMode.Open, storeForApplication))
          {
            using (BinaryReader reader = new BinaryReader((Stream) input))
            {
              short version = reader.ReadInt16();
              if (version > (short) 1)
                throw new Exception("wrong version " + version.ToString((IFormatProvider) CultureInfo.CurrentCulture));
              reader.ReadString();
              this.myGame.LoadFromReader(reader, (int) version);
              GC.Collect();
            }
          }
        }
      }
      catch
      {
        this.overlayText = "load error...";
        this.overlayTimer = 200;
      }
    }

    public DateTime GetSaveDate(int index)
    {
      string path = "mcsave" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(path))
          {
            using (IsolatedStorageFileStream input = new IsolatedStorageFileStream("mcsave" + index.ToString((IFormatProvider) CultureInfo.InvariantCulture), FileMode.Open, storeForApplication))
            {
              using (BinaryReader binaryReader = new BinaryReader((Stream) input))
              {
                if (binaryReader.ReadInt16() <= (short) 1)
                {
                  DateTime result;
                  if (DateTime.TryParse(binaryReader.ReadString(), (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                    return result;
                }
              }
            }
          }
          return DateTime.MinValue;
        }
      }
      catch
      {
        return DateTime.MinValue;
      }
    }

    public void Quit() => this.Exit();

    public void ClearInput()
    {
      this.fUpId = 0;
      this.fDownId = 0;
      this.fLeftId = 0;
      this.fRightId = 0;
      this.fAttackID = 0;
      this.fMenuID = 0;

      //RnD
      //InputHandler.ReleaseAll();
    }

    private void GameDeactivated(object sender, EventArgs e)
    {
        this.SaveGame(9);
    }

    //RnD
    private void GameActivated(object sender, EventArgs e)
    {
    }


    // -------------------------------------
    public void Resize()
    {
        this.WIDTH = miniprefs.GameWidth;
        this.HEIGHT = miniprefs.GameHeight;
        if (this.graphicsDevice == null)
            return;
        this.initVideo();
    }

    public Texture2D Texture
    {
        get
        {
            return this.buffer1Active ? this.myRenderTarget1 : this.myRenderTarget2;
        }
    }

    public void SetMenu(Menu menu)
    {
        this.ActiveMenu = menu;
        menu?.Init(this, this.Input);
        this.OnClearInput();
    }

    public void ResetGame()
    {
        this.playerDeadTime = 0;
        this.wonTimer = 0;
        this.GameTime = 0;
        this.HasWon = false;
        this.levels = new Level[5];
        this.currentLevel = 3;
        this.levels[4] = new Level(128, 128, 1, (Level)null, this);
        this.levels[3] = new Level(128, 128, 0, this.levels[4], this);
        this.levels[2] = new Level(128, 128, -1, this.levels[3], this);
        this.levels[1] = new Level(128, 128, -2, this.levels[2], this);
        this.levels[0] = new Level(128, 128, -3, this.levels[1], this);
        this.level = this.levels[this.currentLevel];
        this.ActivePlayer = new Player(this, this.Input);
        this.ActivePlayer.FindStartPos(this.level);
        this.level.Add((Entity)this.ActivePlayer);
        for (int index = 0; index < this.levels.Length; ++index)
            this.levels[index].TrySpawn(5000);
    }

    public void Init(ContentManager content, GraphicsDevice graphicsDevice)
    {
        this.initAudioVideo(content, graphicsDevice);
        this.ResetGame();
        this.SetMenu((Menu)new TitleMenu());
    }

    public void Init(
        ContentManager content,
        GraphicsDevice graphicsDevice,
        BinaryReader reader,
        int version)
    {
        this.initAudioVideo(content, graphicsDevice);
        this.LoadFromReader(reader, version);
    }

    //RnD
    public void Tick()
    {
        ++this.tickCount;
        if (!this.ActivePlayer.Removed && !this.HasWon)
            ++this.GameTime;

        //RnD
        this.Input.Tick();

        if (this.ActiveMenu != null)
        {
            this.ActiveMenu.Tick();
        }
        else
        {
            if (this.ActivePlayer.Removed)
            {
                ++this.playerDeadTime;
                if (this.playerDeadTime > 60)
                    this.SetMenu((Menu)new DeadMenu());
            }
            else if (this.pendingLevelChange != 0)
            {
                this.SetMenu((Menu)new LevelTransitionMenu(this.pendingLevelChange));
                this.pendingLevelChange = 0;
            }
            if (this.wonTimer > 0 && --this.wonTimer == 0)
                this.SetMenu((Menu)new WonMenu());

            this.level.Tick();
            
            ++Tile.TickCount;
        }
    }

    public void ChangeLevel(int dir)
    {
        this.level.Remove((Entity)this.ActivePlayer);
        this.currentLevel += dir;
        this.level = this.levels[this.currentLevel];
        this.ActivePlayer.X = (this.ActivePlayer.X >> 4) * 16 + 8;
        this.ActivePlayer.Y = (this.ActivePlayer.Y >> 4) * 16 + 8;
        this.level.Add((Entity)this.ActivePlayer);
    }

    public bool EscapeClicked()
    {
        if (this.ActiveMenu == null)
            this.SetMenu((Menu)new PauseMenu(this));
        else if (this.ActiveMenu is PauseMenu)
            this.SetMenu((Menu)null);
        else if (this.ActiveMenu is LoadSaveMenu)
            this.SetMenu(((LoadSaveMenu)this.ActiveMenu).Parent);
        else if (this.ActiveMenu is InstructionsMenu)
            this.SetMenu(((InstructionsMenu)this.ActiveMenu).Parent);
        else if (this.ActiveMenu is AboutMenu)
            this.SetMenu(((AboutMenu)this.ActiveMenu).Parent);
        else if (this.ActiveMenu is OptionsMenu)
            this.SetMenu(((OptionsMenu)this.ActiveMenu).Parent);
        else if (this.ActiveMenu is WonMenu || this.ActiveMenu is DeadMenu)
        {
            this.SetMenu((Menu)new TitleMenu());
        }
        else
        {
            if (this.ActiveMenu is TitleMenu)
                return true;
            this.SetMenu((Menu)null);
        }
        return false;
    }

    public void Render()
    {
        int num1 = this.ActivePlayer.X - this.screen.W / 2;
        int num2 = this.ActivePlayer.Y - (this.screen.H - 8) / 2;
        if (num1 < 16)
            num1 = 16;
        if (num2 < 16)
            num2 = 16;
        if (num1 > this.level.W * 16 - this.screen.W - 16)
            num1 = this.level.W * 16 - this.screen.W - 16;
        if (num2 > this.level.H * 16 - this.screen.H - 16)
            num2 = this.level.H * 16 - this.screen.H - 16;
        if (this.currentLevel > 3)
        {
            int colors = gfx.Color.Get(20, 20, 121, 121);
            int num3 = 16;
            int num4 = 26;
            if (miniprefs.ScreenSize == 1)
            {
                num3 *= 2;
                num4 *= 2;
            }
            for (int index1 = 0; index1 < num3; ++index1)
            {
                for (int index2 = 0; index2 < num4; ++index2)
                    this.screen.Render(index2 * 8 - (num1 / 4 & 7), index1 * 8 - (num2 / 4 & 7), 0, colors, 0);
            }
        }
        this.level.RenderBackground(this.screen, num1, num2);
        this.level.RenderSprites(this.screen, num1, num2);
        if (this.currentLevel < 3)
        {
            this.lightScreen.Clear(0);
            this.level.RenderLight(this.lightScreen, num1, num2);
            this.screen.Overlay(this.lightScreen, num1, num2);
        }
        this.renderGui();
        for (int index3 = 0; index3 < this.screen.H; ++index3)
        {
            for (int index4 = 0; index4 < this.screen.W; ++index4)
            {
                int pixel = this.screen.Pixels[index4 + index3 * this.screen.W];
                if (pixel < (int)byte.MaxValue)
                    this.destinationColors[index4 + index3 * this.WIDTH] = this.colors[pixel];
            }
        }
        if (this.buffer1Active)
            this.myRenderTarget2.SetData<Microsoft.Xna.Framework.Color>(this.destinationColors);
        else
            this.myRenderTarget1.SetData<Microsoft.Xna.Framework.Color>(this.destinationColors);
        this.buffer1Active = !this.buffer1Active;
    }

    public void ScheduleLevelChange(int dir)
    {
        this.pendingLevelChange = dir;
        this.OnAutosave();
    }

    public void Won()
    {
        this.wonTimer = 180;
        this.HasWon = true;
    }

    public void SaveToWriter(BinaryWriter writer)
    {
        writer.Write(this.tickCount);
        writer.Write(this.GameTime);
        writer.Write(this.currentLevel);
        writer.Write(this.playerDeadTime);
        writer.Write(this.pendingLevelChange);
        writer.Write(this.wonTimer);
        writer.Write(this.HasWon);
        this.ActivePlayer.SaveToWriter(this, writer);
        if (this.ActiveMenu != null)
        {
            writer.Write(true);
            this.ActiveMenu.SaveToWriter(writer);
        }
        else
            writer.Write(false);
        int num = -1;
        for (int index = 0; index < this.levels.Length; ++index)
        {
            this.levels[index].SaveToWriter(this, writer);
            if (this.levels[index] == this.level)
                num = index;
        }
        writer.Write(num);
    }

    public void LoadFromReader(BinaryReader reader, int version)
    {
        this.tickCount = reader.ReadInt32();
        this.GameTime = reader.ReadInt32();
        this.currentLevel = reader.ReadInt32();
        this.playerDeadTime = reader.ReadInt32();
        this.pendingLevelChange = reader.ReadInt32();
        this.wonTimer = reader.ReadInt32();
        this.HasWon = reader.ReadBoolean();
        this.ActivePlayer = (Player)Entity.NewEntityFromReader(this, reader);
        this.ActiveMenu = !reader.ReadBoolean() ? (Menu)null : Menu.NewMenuFromReader(this, reader);
        this.levels = new Level[5];
        for (int index = 0; index < this.levels.Length; ++index)
            this.levels[index] = new Level(this, reader, version);
        this.level = this.levels[reader.ReadInt32()];
    }

    public void SaveLevelNumToWriter(Level level, BinaryWriter writer)
    {
        if (level != null)
        {
            for (int index = 0; index < this.levels.Length; ++index)
            {
                if (this.levels[index] == level)
                {
                    writer.Write(index);
                    return;
                }
            }
        }
        writer.Write(-1);
    }

    public DateTime GetSaveDate1(int index)
    {
        return this.OnGetSaveDate != null ? this.OnGetSaveDate(index) : DateTime.MinValue;
    }

    public void SaveGame1(int index)
    {
        if (this.OnSaveGame == null)
            return;
        this.OnSaveGame(index);
    }

    public void LoadGame1(int index)
    {
        if (this.OnLoadGame == null)
            return;
        this.OnLoadGame(index);
    }

    public Game1.GameStatus GetGameStatus()
    {
        if (this.ActiveMenu == null)
        {
            if (this.ActivePlayer.CheckUse())
                return this.ActivePlayer.ActiveItem is PowerGloveItem
                                ? Game1.GameStatus.Game_UseGlove
                                : Game1.GameStatus.Game_Use;

            if (this.ActivePlayer.ActiveItem is FurnitureItem)
                return Game1.GameStatus.Game_Place;
            if (this.ActivePlayer.ActiveItem is PowerGloveItem)
                return Game1.GameStatus.Game_Glove;
            if (this.ActivePlayer.ActiveItem is ToolItem)
            {
                ToolItem activeItem = (ToolItem)this.ActivePlayer.ActiveItem;
                if (activeItem.MyType == ToolType.Axe)
                    return Game1.GameStatus.Game_Axe;
                if (activeItem.MyType == ToolType.Hoe)
                    return Game1.GameStatus.Game_Hoe;
                if (activeItem.MyType == ToolType.Pickaxe)
                    return Game1.GameStatus.Game_Pickaxe;

                return activeItem.MyType == ToolType.Shovel
                                ? Game1.GameStatus.Game_Shovel
                                : Game1.GameStatus.Game_Attack;
            }
            return this.ActivePlayer.ActiveItem is ResourceItem
                        ? Game1.GameStatus.Game_UseResource
                        : Game1.GameStatus.Game_Attack;
        }

        if (this.ActiveMenu is LoadSaveMenu || this.ActiveMenu is PauseMenu
                    || this.ActiveMenu is OptionsMenu)
        {
            return Game1.GameStatus.Menu_Back;
        }

        if (this.ActiveMenu is AboutMenu || this.ActiveMenu is DeadMenu
                    || this.ActiveMenu is InstructionsMenu || this.ActiveMenu is WonMenu)
            return Game1.GameStatus.Menu_BackOnly;

        if (this.ActiveMenu is CraftingMenu)
            return Game1.GameStatus.Menu_Crafting;

        if (this.ActiveMenu is InventoryMenu)
            return Game1.GameStatus.Menu_Inventory;

        return this.ActiveMenu is ContainerMenu
                    ? Game1.GameStatus.Menu_Chest : Game1.GameStatus.Menu_Select;
    }

    public Game1.MiniMapTile GetMiniMap(int x, int y)
    {
        if (this.level == null || x < 0 || y < 0 || x >= this.level.W || y >= this.level.H)
            return Game1.MiniMapTile.notOnMap;
        int index = x + y * this.level.H;
        if (!this.level.Seen[index])
            return Game1.MiniMapTile.unknown;
        switch (this.level.Tiles[index])
        {
            case 0:
                return Game1.MiniMapTile.Grass;
            case 1:
                return Game1.MiniMapTile.Rock;
            case 2:
                return Game1.MiniMapTile.Water;
            case 3:
                return Game1.MiniMapTile.Flower;
            case 4:
                return Game1.MiniMapTile.Tree;
            case 5:
                return Game1.MiniMapTile.Dirt;
            case 6:
                return Game1.MiniMapTile.Sand;
            case 7:
                return Game1.MiniMapTile.Cactus;
            case 8:
                return Game1.MiniMapTile.Hole;
            case 9:
                return Game1.MiniMapTile.TreeSapling;
            case 10:
                return Game1.MiniMapTile.CactusSapling;
            case 11:
                return Game1.MiniMapTile.Farmland;
            case 12:
                return Game1.MiniMapTile.Wheat;
            case 13:
                return Game1.MiniMapTile.Lava;
            case 14:
                return Game1.MiniMapTile.StairsDown;
            case 15:
                return Game1.MiniMapTile.StairsUp;
            case 16:
                return Game1.MiniMapTile.InfiniteFall;
            case 17:
                return Game1.MiniMapTile.Cloud;
            case 18:
                return Game1.MiniMapTile.HardRock;
            case 19:
                return Game1.MiniMapTile.IronOre;
            case 20:
                return Game1.MiniMapTile.GoldOre;
            case 21:
                return Game1.MiniMapTile.GemOre;
            case 22:
                return Game1.MiniMapTile.CloudCactus;
            default:
                return Game1.MiniMapTile.unknown;
        }
    }

    private void initAudioVideo(ContentManager content, GraphicsDevice graphicsDevice)
    {
        this.content = content;
        this.graphicsDevice = graphicsDevice;
        int num1 = 0;
        for (int index1 = 0; index1 < 6; ++index1)
        {
            for (int index2 = 0; index2 < 6; ++index2)
            {
                for (int index3 = 0; index3 < 6; ++index3)
                {
                    int num2 = index1 * (int)byte.MaxValue / 5;
                    int num3 = index2 * (int)byte.MaxValue / 5;
                    int num4 = index3 * (int)byte.MaxValue / 5;
                    int num5 = (num2 * 30 + num3 * 59 + num4 * 11) / 100;
                    int r = (num2 + num5) / 2 * 230 / (int)byte.MaxValue + 10;
                    int g = (num3 + num5) / 2 * 230 / (int)byte.MaxValue + 10;
                    int b = (num4 + num5) / 2 * 230 / (int)byte.MaxValue + 10;
                    this.colors[num1++] = new Microsoft.Xna.Framework.Color(r, g, b);
                }
            }
        }
        Sound.InitSound(content);
        this.initVideo();
    }

    private void initVideo()
    {
        this.myRenderTarget1 = new Texture2D(this.graphicsDevice, this.WIDTH, this.HEIGHT);
        this.myRenderTarget2 = new Texture2D(this.graphicsDevice, this.WIDTH, this.HEIGHT);
        this.destinationColors = new Microsoft.Xna.Framework.Color[this.WIDTH * this.HEIGHT];
        this.screen = new Screen(this.WIDTH, this.HEIGHT, new SpriteSheet(this.content, "gfx\\icons"));
        this.lightScreen = new Screen(this.WIDTH, this.HEIGHT, new SpriteSheet(this.content, "gfx\\icons"));
    }

    private void renderGui()
    {
        for (int index1 = 0; index1 < 2; ++index1)
        {
            for (int index2 = 0; index2 < 20; ++index2)
                this.screen.Render(index2 * 8, this.screen.H - 16 + index1 * 8, 384, gfx.Color.Get(0, 0, 0, 0), 0);
        }
        for (int index = 0; index < 10; ++index)
        {
            if (index < this.ActivePlayer.Health)
                this.screen.Render(index * 8, this.screen.H - 16, 384, gfx.Color.Get(0, 200, 500, 533), 0);
            else
                this.screen.Render(index * 8, this.screen.H - 16, 384, gfx.Color.Get(0, 100, 0, 0), 0);

            if (this.ActivePlayer.StaminaRechargeDelay > 0)
            {
                if (this.ActivePlayer.StaminaRechargeDelay / 4 % 2 == 0)
                    this.screen.Render(index * 8, this.screen.H - 8, 385, gfx.Color.Get(0, 555, 0, 0), 0);
                else
                    this.screen.Render(index * 8, this.screen.H - 8, 385, gfx.Color.Get(0, 110, 0, 0), 0);
            }
            else if (index < this.ActivePlayer.Stamina)
                this.screen.Render(index * 8, this.screen.H - 8, 385, gfx.Color.Get(0, 220, 550, 553), 0);
            else
                this.screen.Render(index * 8, this.screen.H - 8, 385, gfx.Color.Get(0, 110, 0, 0), 0);
        }
        if (this.ActivePlayer.ActiveItem != null)
            this.ActivePlayer.ActiveItem.RenderInventory(this.screen, 80, this.screen.H - 16);
        if (this.ActiveMenu == null)
            return;
        this.ActiveMenu.Render(this.screen);
    }

    // -------------------------------------

    public enum StartStatus
    {
      logoStart,
      readyForGame,
      game,
    }


    //RnD
    public enum GameStatus
    {
        Menu_Select,
        Menu_Back,
        Menu_BackOnly,
        Menu_Inventory,
        Menu_Crafting,
        Menu_Chest,
        Game_Attack,
        Game_Glove,
        Game_Axe,
        Game_Hoe,
        Game_Pickaxe,
        Game_Shovel,
        Game_Use,
        Game_UseGlove,
        Game_UseResource,
        Game_Place,
    }


    public enum MiniMapTile
    {
        notOnMap,
        unknown,
        Grass,
        Rock,
        Water,
        Flower,
        Tree,
        Dirt,
        Sand,
        Cactus,
        Hole,
        TreeSapling,
        CactusSapling,
        Farmland,
        Wheat,
        Lava,
        StairsDown,
        StairsUp,
        InfiniteFall,
        Cloud,
        HardRock,
        IronOre,
        GoldOre,
        GemOre,
        CloudCactus,
     }

     public delegate DateTime GetSaveDateDelegate(int index);
         
     public delegate void SaveGameDelegate(int index);

     public delegate void LoadGameDelegate(int index);

     public delegate void ClearInputDelegate();

     public delegate void AutosaveDelegate();

    }//class end
}//namespace end
