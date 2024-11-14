// Decompiled with JetBrains decompiler
// Type: GameEngine.GameScreen
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch; //!
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public abstract class GameScreen
  {
    protected SceneNode sceneGraph;

    public Dictionary<string, Rectangle> spriteSheetDescriptorList 
            = new Dictionary<string, Rectangle>();

    public ContentManager content;
    public TextureManager textureManager;
    public AudioManager audioManager;
    public bool useContentManager;
    public bool suppressSpritesheetWarning;
    public bool isDisabled;
    public bool IsPaused;
    public bool PauseScenegraphUpdates;
    protected Matrix worldTransform = Matrix.Identity;
    public bool Covered;
    protected int CoverAlpha = (int) sbyte.MaxValue;
    public bool isPopup = true;
    public bool handleInput = true;
    public SpriteBatch spriteBatch;
    public string Name;
    public bool blockInput;
    public bool removeAnimsOnExit = true;
    public TimeSpan TransitionOnTime = TimeSpan.Zero;
    public TimeSpan TransitionOffTime = TimeSpan.Zero;
    protected float TransitionPosition = 1f;
    public ScreenState screenState;
    public bool IsExiting;
    private bool otherScreenHasFocus;
    public ScreenManager screenManager;

    public byte TransitionAlpha
    {
      get
      {
        return (byte) ((double) byte.MaxValue - (double) this.TransitionPosition * (double) byte.MaxValue);
      }
    }

    public bool IsActive
    {
      get
      {
        if (this.otherScreenHasFocus)
          return false;
        return this.screenState == ScreenState.TransitionOn || this.screenState == ScreenState.Active;
      }
    }

    public GameScreen()
      : this("gamescreen", false)
    {
    }

    public GameScreen(string name)
      : this(name, false)
    {
    }

    public GameScreen(string name, bool useCM)
    {
      this.Name = name;
      this.sceneGraph = new SceneNode("ROOT", this);
      this.audioManager = new AudioManager(this);
      this.textureManager = new TextureManager(this);
      this.useContentManager = useCM;
    }

    ~GameScreen()
    {
    }

    public void TransitionTime(float on, float off)
    {
      this.TransitionOnTime = TimeSpan.FromSeconds((double) on);
      this.TransitionOffTime = TimeSpan.FromSeconds((double) off);
      if ((double) on != 0.0)
        return;
      this.TransitionPosition = 0.0f;
      this.CoverAlpha = 0;
    }

    public virtual void OnScreenLoaded()
    {
    }

    public virtual void Activate(bool instancePreserved)
    {
      if (!instancePreserved)
      {
        this.LoadContent();
        this.Initialize();
      }
      else
        this.OnFASActivate();
    }

    public virtual void OnFASActivate()
    {
    }

    public virtual void OnDeactivate()
    {
    }

    public virtual void Close()
    {
    }

    public virtual void Initialize()
    {
    }

    public virtual void LoadContent()
    {
      if (this.content == null)
        this.content = new ContentManager((IServiceProvider) this.screenManager.Game.Services, 
            "Content");
      this.spriteBatch = new SpriteBatch(Engine.gdm.GraphicsDevice);
    }

    public virtual void UnloadContent()
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) this.sceneGraph)
        sceneNode.Unload();
      this.sceneGraph.Clear();
      if (this.content != null)
        this.content.Unload();
      this.textureManager.Unload();
    }

    public virtual void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      this.audioManager.Update(gameTime);
      if (!this.PauseScenegraphUpdates)
        this.sceneGraph.Update(gameTime, ref this.worldTransform);
      this.Covered = coveredByOtherScreen;
      this.otherScreenHasFocus = otherScreenHasFocus;
      if (this.IsExiting)
      {
        this.screenState = ScreenState.TransitionOff;
        if (this.UpdateTransition(gameTime, this.TransitionOffTime, 1))
          return;
        this.screenManager.RemoveScreen(this);
      }
      else if (this.UpdateTransition(gameTime, this.TransitionOnTime, -1))
      {
        this.screenState = ScreenState.TransitionOn;
      }
      else
      {
        if (this.screenState != ScreenState.Active)
          this.OnScreenLoaded();
        this.screenState = ScreenState.Active;
      }
    }

    protected bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
    {
      this.TransitionPosition += (!(time == TimeSpan.Zero) 
                ? (float) (gameTime.ElapsedGameTime.TotalMilliseconds / time.TotalMilliseconds) 
                : 1f) * (float) direction;
      if ((direction >= 0 || (double) this.TransitionPosition > 0.0) 
                && (direction <= 0 || (double) this.TransitionPosition < 1.0))
        return true;
      this.TransitionPosition = MathHelper.Clamp(this.TransitionPosition, 0.0f, 1f);
      return false;
    }

    public virtual void HandleInput(GestureSample gesture)
    {
      if (!this.handleInput)
        return;
      this.sceneGraph.HandleInput(gesture);
    }

    public bool BackKeyPressed()
    {
      return GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;
    }

    public virtual void Draw(GameTime gameTime)
    {
      this.sceneGraph.Draw();
      this.screenManager.FadeBackBufferToBlack((int) byte.MaxValue - (int) this.TransitionAlpha);
      if (!this.Covered)
        return;
      this.screenManager.FadeBackBufferToBlack(this.CoverAlpha);
    }

    public void ExitScreen()
    {
      this.handleInput = false;
      this.OnScreenExiting();
      if (this.TransitionOffTime == TimeSpan.Zero)
        this.screenManager.RemoveScreen(this);
      else
        this.IsExiting = true;
    }

    public virtual void OnScreenExiting()
    {
    }

    public GameScreen GetScreenByName(string name)
    {
        return this.screenManager.GetScreenByName(name);
    }

    public T GetScreenByName<T>(string name) where T : GameScreen
    {
      return this.screenManager.GetScreenByName<T>(name);
    }

    public T GetScreenByType<T>() where T : GameScreen => this.screenManager.GetScreenByType<T>();

    public bool AddSpriteSheet(params string[] XMLfilename)
    {
      SpriteSheetDescriptor items = new SpriteSheetDescriptor();
      foreach (string filename in XMLfilename)
      {
        if (!DataStore.ExternalDeserialize<SpriteSheetDescriptor>(filename, out items))
          return false;
        for (int index = 0; index < items.nameList.Count; ++index)
        {
          if (!this.spriteSheetDescriptorList.ContainsKey(items.nameList[index].ToUpperInvariant()))
            this.spriteSheetDescriptorList.Add(items.nameList[index].ToUpperInvariant(), items.sourceRectList[index]);
        }
      }
      return true;
    }

    public bool GetSpriteSource(string spritename, ref Rectangle rect)
    {
      if (!this.spriteSheetDescriptorList.ContainsKey(spritename.ToUpperInvariant()))
        return false;
      rect = this.spriteSheetDescriptorList[spritename.ToUpperInvariant()];
      return true;
    }

    public Rectangle GetSpriteSource(string spritename)
    {
      return this.spriteSheetDescriptorList.ContainsKey(spritename.ToUpperInvariant()) 
                ? this.spriteSheetDescriptorList[spritename.ToUpperInvariant()] 
                : Rectangle.Empty;
    }

    public void AssignSourceRects(SceneNode searchNode)
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) searchNode)
        sceneNode.SetSourceRect();
    }
  }
}
