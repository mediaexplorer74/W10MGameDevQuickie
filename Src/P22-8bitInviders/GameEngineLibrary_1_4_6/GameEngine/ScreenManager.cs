// Decompiled with JetBrains decompiler
// Type: GameEngine.ScreenManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using GameManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch; //!
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace GameEngine
{
  public class ScreenManager : DrawableGameComponent
  {
    public TouchInputManager touchInputManager;
    private List<GameScreen> screens = new List<GameScreen>();
    private List<GameScreen> tempScreensList = new List<GameScreen>();
    public SpriteBatch SpriteBatch;
    public SpriteFont Font;
    public Color FPSColor = Color.White;
    public int PauseAlpha = (int) sbyte.MaxValue;
    private Texture2D blankTexture;
    private bool capturedPause;
    private bool isInitialized;
    private Matrix GT;


    public ScreenManager(Game game)
      : base(game)
    {
      this.touchInputManager = new TouchInputManager();

            //RnD
       GT = Game1.globalTransformation;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.isInitialized = true;
    }

    protected override void LoadContent()
    {
      this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.Font = Engine.GlobalContent.Load<SpriteFont>("GameEngine/defaultfont");
      this.blankTexture = new Texture2D(Engine.gdm.GraphicsDevice, 1, 1);
      this.blankTexture.SetData<Color>(new Color[1]
      {
        Color.White
      });
      foreach (GameScreen screen in this.screens)
      {
        screen.LoadContent();
        screen.Initialize();
      }
    }

    public override void Update(GameTime gameTime)
    {
      this.touchInputManager.Update();

      GestureSample gesture = new GestureSample();

      while (TouchPanel.IsGestureAvailable)
        gesture = TouchPanel.ReadGesture();
     
      if (Engine.isObscured)
        return;

      TimerManager.Instance.Update(gameTime);
      this.tempScreensList.Clear();

      foreach (GameScreen screen in this.screens)
      {
        if (!screen.isDisabled)
          this.tempScreensList.Add(screen);
      }
      bool otherScreenHasFocus = false;
      bool coveredByOtherScreen = false;
      bool flag = false;
      while (this.tempScreensList.Count > 0)
      {
        GameScreen tempScreens = this.tempScreensList[this.tempScreensList.Count - 1];
        this.tempScreensList.RemoveAt(this.tempScreensList.Count - 1);
        tempScreens.Covered = coveredByOtherScreen;
        tempScreens.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        if (tempScreens.screenState == ScreenState.TransitionOn 
                    || tempScreens.screenState == ScreenState.Active)
        {
          if (!otherScreenHasFocus)
          {
            if (tempScreens.handleInput && !flag)
              tempScreens.HandleInput(gesture);
            if (tempScreens.blockInput)
              flag = true;
          }
          if (!tempScreens.isPopup)
            coveredByOtherScreen = true;
        }
      }
    }


    public override void Draw(GameTime gameTime)
    {
      if (Engine.isObscured)
      {
        if (!this.capturedPause)
        {
          this.capturedPause = true;

              //RnD : 0, RenderTargetUsage.PreserveContents
              RenderTarget2D renderTarget = new RenderTarget2D(
              Engine.gdm.GraphicsDevice, Engine.gameWidth, Engine.gameHeight, 
              false, SurfaceFormat.Color, DepthFormat.Depth16, 0, RenderTargetUsage.PreserveContents);
                   

                    Engine.gdm.GraphicsDevice.SetRenderTarget(renderTarget);

          foreach (GameScreen screen in this.screens)
          {
            if (!screen.isDisabled)
              screen.Draw(gameTime);
          }
          Engine.gdm.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
          PauseOverlay.frozen = (Texture2D) renderTarget;
        }
        PauseOverlay.Draw(this.SpriteBatch, this.PauseAlpha);
      }
      else
      {
        this.capturedPause = false;
        foreach (GameScreen screen in this.screens)
        {
          if (!screen.isDisabled)
            screen.Draw(gameTime);
        }
      }
    }

    public void AddScreen(GameScreen screen)
    {
      screen.screenManager = this;
      screen.IsExiting = false;

      if (this.isInitialized)
        screen.Activate(false);

      this.screens.Add(screen);
    }

    public void RemoveScreen(GameScreen screen)
    {
      if (screen.removeAnimsOnExit)
        Engine.aniManager.RemoveAll();
      if (this.isInitialized)
        screen.UnloadContent();
      this.screens.Remove(screen);
      this.tempScreensList.Remove(screen);
    }

    public GameScreen[] GetScreens()
    {
        return this.screens.ToArray();
    }

    public GameScreen GetScreenByName(string name)
    {
      foreach (GameScreen screen in this.screens)
      {
        if (screen.Name == name)
          return screen;
      }
      return (GameScreen) null;
    }

    public T GetScreenByName<T>(string name) where T : GameScreen
    {
      foreach (GameScreen screen in this.screens)
      {
        if (screen.Name == name)
          return (T) screen;
      }
      return default (T);
    }

    public T GetScreenByType<T>() where T : GameScreen
    {
      foreach (GameScreen screen in this.screens)
      {
        if (screen is T screenByType)
          return screenByType;
      }
      return default (T);
    }

    public void FadeBackBufferToBlack(int alpha)
    {
      //this.SpriteBatch.Begin();
      this.SpriteBatch.Begin(SpriteSortMode.Deferred/*.BackToFront*/, null,
        null, null, null, null, Game1.globalTransformation);

            if (GT != Game1.globalTransformation)
            {
                Debug.WriteLine("[i] GT changed: " + Game1.globalTransformation);
                GT = Game1.globalTransformation;
            }

      this.SpriteBatch.Draw(this.blankTexture, 
          new Rectangle(0, 0, Engine.gameWidth, Engine.gameHeight), 
          new Color(0, 0, 0, (int) (byte) alpha));
      this.SpriteBatch.End();
    }

    public void OnActivating(bool instancePreserved)
    {
      if (!instancePreserved)
        return;

      this.tempScreensList.Clear();

      foreach (GameScreen screen in this.screens)
        this.tempScreensList.Add(screen);

      foreach (GameScreen tempScreens in this.tempScreensList)
        tempScreens.Activate(true);
    }

    public void OnDeactivating()
    {
      this.tempScreensList.Clear();

      foreach (GameScreen screen in this.screens)
        this.tempScreensList.Add(screen);

      foreach (GameScreen tempScreens in this.tempScreensList)
        tempScreens.OnDeactivate();
    }

    public void OnClosing()
    {
      this.tempScreensList.Clear();

      foreach (GameScreen screen in this.screens)
        this.tempScreensList.Add(screen);

      foreach (GameScreen tempScreens in this.tempScreensList)
        tempScreens.Close();
    }
  }
}
