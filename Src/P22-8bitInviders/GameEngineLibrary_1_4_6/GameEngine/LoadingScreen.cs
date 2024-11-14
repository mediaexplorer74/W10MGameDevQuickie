// Decompiled with JetBrains decompiler
// Type: GameEngine.LoadingScreen
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using GameManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

#nullable disable
namespace GameEngine
{
  public sealed class LoadingScreen : GameScreen
  {
    private static string[] fontContent = (string[]) null;
    private bool loadingIsSlow;
    private bool otherScreensAreGone;
    private static string customLoadImage;
    private static string customText;
    private static TimeSpan logoTime;
    private static int waitTime;
    private GameScreen[] screensToLoad;
    private Texture2D loadImage;
    //private static Matrix GT;

    private LoadingScreen(float duration, bool loadingIsSlow, 
        GameScreen[] screensToLoad)
      : base(nameof (LoadingScreen))
    {
      //GT = globalTransformation;
      this.loadingIsSlow = loadingIsSlow;
      this.screensToLoad = screensToLoad;
      this.removeAnimsOnExit = false;
      this.TransitionOnTime = TimeSpan.FromSeconds((double) duration);
    }

    public static void Load(float duration, string customImage, params GameScreen[] screensToLoad)
    {
      Tools.Trace("Loading Screen");
      string empty = string.Empty;
      LoadingScreen.fontContent = new string[0];
      LoadingScreen.customLoadImage = customImage;
      LoadingScreen.customText = empty;
      foreach (GameScreen screen in Engine.screenManager.GetScreens())
      {
            screen.ExitScreen();
      }
      bool loadingIsSlow = !string.IsNullOrEmpty(customImage) || !string.IsNullOrEmpty(empty);
      
      //RnD: GT
      LoadingScreen screen1 = new LoadingScreen(duration, loadingIsSlow, screensToLoad); 
      
      Engine.screenManager.AddScreen((GameScreen) screen1);
    }

    public static void Load(params GameScreen[] screensToLoad)
    {
      LoadingScreen.waitTime = 0;
      LoadingScreen.logoTime = TimeSpan.FromTicks(DateTime.Now.Ticks);
      LoadingScreen.Load(0.0f, string.Empty, screensToLoad);
    }

    public static void LoadLogo(float duration, params GameScreen[] screensToLoad)
    {
      LoadingScreen.waitTime = 2 + (int) duration;
      LoadingScreen.logoTime = TimeSpan.FromTicks(DateTime.Now.Ticks);
      LoadingScreen.Load(duration, "gameengine/glowpuff2013", screensToLoad);
    }

    public static void AddGlobalFonts(params string[] fontcontent)
    {
      LoadingScreen.fontContent = new string[fontcontent.Length];
      for (int index = 0; index < fontcontent.Length; ++index)
        LoadingScreen.fontContent[index] = fontcontent[index];
    }

    public override void LoadContent()
    {
      foreach (string assetName in LoadingScreen.fontContent)
        Engine.GlobalContent.Load<SpriteFont>(assetName);
      if (this.content != null || string.IsNullOrEmpty(LoadingScreen.customLoadImage))
        return;
      this.loadImage = this.textureManager.Load(LoadingScreen.customLoadImage);
    }

    public override void UnloadContent()
    {
      if (this.content != null)
        this.content.Unload();
      this.textureManager.Unload();
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
      TimeSpan timeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks);
      if (!this.otherScreensAreGone 
        || !(timeSpan - LoadingScreen.logoTime >= TimeSpan.FromSeconds((double) LoadingScreen.waitTime)))
        return;
      
      TimerManager.Instance.TimerList.Clear();
      TimerManager.Instance.StopwatchTimerList.Clear();
      this.screenManager.RemoveScreen((GameScreen) this);

      foreach (GameScreen screen in this.screensToLoad)
      {
        if (screen != null)
          this.screenManager.AddScreen(screen);
      }
      this.screenManager.Game.ResetElapsedTime();
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.screenState == ScreenState.Active && this.screenManager.GetScreens().Length == 1)
        this.otherScreensAreGone = true;
      if (!this.loadingIsSlow)
        return;
      SpriteFont font = this.screenManager.Font;
      Vector2 vector2_1 = new Vector2((float) Engine.gameWidth, (float) Engine.gameHeight);
      Vector2 vector2_2 = font.MeasureString(LoadingScreen.customText);
      Vector2 position = (vector2_1 - vector2_2) / 2f;
      float num = (float) Convert.ToInt32(this.TransitionAlpha) / (float) byte.MaxValue;
      Color color = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);

      //this.screenManager.SpriteBatch.Begin();
      this.screenManager.SpriteBatch.Begin(SpriteSortMode.Deferred/*.BackToFront*/, null,
        null, null, null, null, Game1.globalTransformation);

      this.screenManager.SpriteBatch.DrawString(font, LoadingScreen.customText, position, color * num);
      if (this.loadImage != null)
        this.screenManager.SpriteBatch.Draw(this.loadImage, 
            new Rectangle(((int) vector2_1.X - this.loadImage.Width) / 2, 
            ((int) vector2_1.Y - this.loadImage.Height) / 2, 
            this.loadImage.Width, this.loadImage.Height), color * num);
      this.screenManager.SpriteBatch.End();
    }
  }
}
