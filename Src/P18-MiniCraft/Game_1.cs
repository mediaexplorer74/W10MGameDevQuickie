// Decompiled with JetBrains decompiler
// Type: GameManager.Game

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameManager.entity;
using GameManager.gfx;
using GameManager.item;
using GameManager.level;
using GameManager.level.tile;
using GameManager.screen;
using GameManager.sound;
using System;
using System.IO;


#nullable disable
namespace GameManager
{
    /*
  public class Game1: Game
  {
    public const string NAME = "Minicraft";
    public const int LevelCount = 5;

    public int HEIGHT;
    public int WIDTH;

    // DISPLAY
    const int SCREENWIDTH = 1024, SCREENHEIGHT = 768;     // TARGET FORMAT
    const bool FULLSCREEN = false;                        // not fullscreen because using windowed fill-screen mode 
    GraphicsDeviceManager graphics;
    
    public readonly InputHandler Input = new InputHandler();
    
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

    public Game1()
    {
        // Set a display mode that is windowed but is the same as the desktop's current resolution (don't show a border)... 
        // This is done instead of using true fullscreen mode since some firewalls will crash the computer in true fullscreen mode
        int initial_screen_width = SCREENWIDTH;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 10; //(-10 if dubugging) 
        int initial_screen_height = SCREENHEIGHT;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height- 10; //(-10 if debugging) [makes taskbar visible]
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = initial_screen_width,
            PreferredBackBufferHeight = initial_screen_height,
            IsFullScreen = FULLSCREEN,
            PreferredDepthStencilFormat = DepthFormat.Depth16
        };
        //Window.IsBorderless = true;
        Content.RootDirectory = "Content";

        //RnD
        this.Resize();
     }

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
      this.levels[4] = new Level(128, 128, 1, (Level) null, this);
      this.levels[3] = new Level(128, 128, 0, this.levels[4], this);
      this.levels[2] = new Level(128, 128, -1, this.levels[3], this);
      this.levels[1] = new Level(128, 128, -2, this.levels[2], this);
      this.levels[0] = new Level(128, 128, -3, this.levels[1], this);
      this.level = this.levels[this.currentLevel];
      this.ActivePlayer = new Player(this, this.Input);
      this.ActivePlayer.FindStartPos(this.level);
      this.level.Add((Entity) this.ActivePlayer);
      for (int index = 0; index < this.levels.Length; ++index)
        this.levels[index].TrySpawn(5000);
    }

    public void Init(ContentManager content, GraphicsDevice graphicsDevice)
    {
      this.initAudioVideo(content, graphicsDevice);
      this.ResetGame();
      this.SetMenu((Menu) new TitleMenu());
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

    public void Tick()
    {
      ++this.tickCount;
      if (!this.ActivePlayer.Removed && !this.HasWon)
        ++this.GameTime;
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
            this.SetMenu((Menu) new DeadMenu());
        }
        else if (this.pendingLevelChange != 0)
        {
          this.SetMenu((Menu) new LevelTransitionMenu(this.pendingLevelChange));
          this.pendingLevelChange = 0;
        }
        if (this.wonTimer > 0 && --this.wonTimer == 0)
          this.SetMenu((Menu) new WonMenu());
        this.level.Tick();
        ++Tile.TickCount;
      }
    }

    public void ChangeLevel(int dir)
    {
      this.level.Remove((Entity) this.ActivePlayer);
      this.currentLevel += dir;
      this.level = this.levels[this.currentLevel];
      this.ActivePlayer.X = (this.ActivePlayer.X >> 4) * 16 + 8;
      this.ActivePlayer.Y = (this.ActivePlayer.Y >> 4) * 16 + 8;
      this.level.Add((Entity) this.ActivePlayer);
    }

    public bool EscapeClicked()
    {
      if (this.ActiveMenu == null)
        this.SetMenu((Menu) new PauseMenu(this));
      else if (this.ActiveMenu is PauseMenu)
        this.SetMenu((Menu) null);
      else if (this.ActiveMenu is LoadSaveMenu)
        this.SetMenu(((LoadSaveMenu) this.ActiveMenu).Parent);
      else if (this.ActiveMenu is InstructionsMenu)
        this.SetMenu(((InstructionsMenu) this.ActiveMenu).Parent);
      else if (this.ActiveMenu is AboutMenu)
        this.SetMenu(((AboutMenu) this.ActiveMenu).Parent);
      else if (this.ActiveMenu is OptionsMenu)
        this.SetMenu(((OptionsMenu) this.ActiveMenu).Parent);
      else if (this.ActiveMenu is WonMenu || this.ActiveMenu is DeadMenu)
      {
        this.SetMenu((Menu) new TitleMenu());
      }
      else
      {
        if (this.ActiveMenu is TitleMenu)
          return true;
        this.SetMenu((Menu) null);
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
          if (pixel < (int) byte.MaxValue)
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
      this.ActivePlayer = (Player) Entity.NewEntityFromReader(this, reader);
      this.ActiveMenu = !reader.ReadBoolean() ? (Menu) null : Menu.NewMenuFromReader(this, reader);
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

    public DateTime GetSaveDate(int index)
    {
      return this.OnGetSaveDate != null ? this.OnGetSaveDate(index) : DateTime.MinValue;
    }

    public void SaveGame(int index)
    {
      if (this.OnSaveGame == null)
        return;
      this.OnSaveGame(index);
    }

    public void LoadGame(int index)
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
          ToolItem activeItem = (ToolItem) this.ActivePlayer.ActiveItem;
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
            int num2 = index1 * (int) byte.MaxValue / 5;
            int num3 = index2 * (int) byte.MaxValue / 5;
            int num4 = index3 * (int) byte.MaxValue / 5;
            int num5 = (num2 * 30 + num3 * 59 + num4 * 11) / 100;
            int r = (num2 + num5) / 2 * 230 / (int) byte.MaxValue + 10;
            int g = (num3 + num5) / 2 * 230 / (int) byte.MaxValue + 10;
            int b = (num4 + num5) / 2 * 230 / (int) byte.MaxValue + 10;
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
  }
*/
}
