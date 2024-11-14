// Decompiled with JetBrains decompiler
// Type: GameManager.AsteroidScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; //!
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class AsteroidScreen : GameScreen
  {
    public AsteroidUINode uiNode;
    public PlayLayer playLayer;
    public AdditiveLayer additiveLayer;
    public Camera2D theCamera;
    private Layer2D starfield;
    private SpriteTile stars;
    private SpriteTile borderLeft;
    private SpriteTile borderRight;
    private SpriteTile borderTop;
    private SpriteTile borderBottom;
    private SpriteTile blON;
    private SpriteTile brON;
    private SpriteTile bbON;
    private SpriteTile btON;
    private float borderOpacity;
    private bool Rehydrate;
    public bool gameOver;
    private float victoryTimer;
    private bool Win;

    public AsteroidScreen()
    {
      this.Rehydrate = true;
      this.TransitionTime(1f, 1f);
      this.AddSpriteSheet("sheets/asteroids.xml", "sheets/additive.xml");
    }

    public override void Initialize()
    {
      this.theCamera = new Camera2D();
      this.theCamera.Position = new Vector2(240f, 400f);
      this.starfield = new Layer2D((GameScreen) this);
      this.starfield.samplerState = SamplerState.LinearWrap;
      this.stars = new SpriteTile("big_starfieldJPG", new Vector2(-100f, -100f), 
          new Rectangle(0, 0, 5000, 5000), "sheets/big_starfield", (GameScreen) this);
      this.starfield.Add((SceneNode) this.stars);
      ASTStarLayer childItem1 = new ASTStarLayer(this.theCamera, (GameScreen) this);
      this.additiveLayer = new AdditiveLayer(this.theCamera, (GameScreen) this);
      this.playLayer = new PlayLayer(this.theCamera, (GameScreen) this);
      Layer2D childItem2 = new Layer2D(this.theCamera, (GameScreen) this);
      childItem2.samplerState = SamplerState.LinearWrap;
      this.borderLeft = new SpriteTile("barrier_off", new Vector2(140f, 0.0f), 
          new Rectangle(0, 0, 64, 5000), "sheets/barrier_off", (GameScreen) this);
      this.borderRight = new SpriteTile("barrier_off", new Vector2(4800f, 0.0f), 
          new Rectangle(0, 0, 64, 5000), "sheets/barrier_off", (GameScreen) this);
      SpriteTile spriteTile1 = new SpriteTile("barrier_off", new Vector2(200f, 200f), 
          new Rectangle(0, 0, 64, 4600), "sheets/barrier_off", (GameScreen) this);
      spriteTile1.Rotation = -90f;
      this.borderTop = spriteTile1;
      SpriteTile spriteTile2 = new SpriteTile("barrier_off", new Vector2(200f, 4860f), 
          new Rectangle(0, 0, 64, 4600), "sheets/barrier_off", (GameScreen) this);
      spriteTile2.Rotation = -90f;
      this.borderBottom = spriteTile2;
      this.blON = new SpriteTile("barrier_on", new Vector2(140f, 0.0f), 
          new Rectangle(0, 0, 64, 5000), "sheets/barrier_on", (GameScreen) this);
      this.brON = new SpriteTile("barrier_on", new Vector2(4800f, 0.0f), 
          new Rectangle(0, 0, 64, 5000), "sheets/barrier_on", (GameScreen) this);
      SpriteTile spriteTile3 = new SpriteTile("barrier_on", new Vector2(200f, 200f),
          new Rectangle(0, 0, 64, 4600), "sheets/barrier_on", (GameScreen) this);
      spriteTile3.Rotation = -90f;

      this.btON = spriteTile3;
      SpriteTile spriteTile4 = new SpriteTile("barrier_on", new Vector2(200f, 4860f), 
          new Rectangle(0, 0, 64, 4600), "sheets/barrier_on", (GameScreen) this);
      spriteTile4.Rotation = -90f;

      this.bbON = spriteTile4;
      childItem2.Add((SceneNode) this.borderLeft);
      childItem2.Add((SceneNode) this.borderTop);
      childItem2.Add((SceneNode) this.borderRight);
      childItem2.Add((SceneNode) this.borderBottom);
      childItem2.Add((SceneNode) this.blON);
      childItem2.Add((SceneNode) this.brON);
      childItem2.Add((SceneNode) this.btON);
      childItem2.Add((SceneNode) this.bbON);
      this.borderLeft.Pivot = Vector3.Zero;
      this.borderRight.Pivot = Vector3.Zero;
      this.borderTop.Pivot = Vector3.Zero;
      this.borderBottom.Pivot = Vector3.Zero;
      this.blON.Pivot = Vector3.Zero;
      this.brON.Pivot = Vector3.Zero;
      this.btON.Pivot = Vector3.Zero;
      this.bbON.Pivot = Vector3.Zero;
      Layer2D childItem3 = new Layer2D((GameScreen) this);
      childItem3.handleInput = true;
      this.uiNode = new AsteroidUINode((GameScreen) this);
      childItem3.Add((SceneNode) this.uiNode);
      this.sceneGraph.Add((SceneNode) this.starfield);
      this.sceneGraph.Add((SceneNode) childItem1);
      this.sceneGraph.Add((SceneNode) childItem2);
      this.sceneGraph.Add((SceneNode) this.additiveLayer);
      this.sceneGraph.Add((SceneNode) this.playLayer);
      this.sceneGraph.Add((SceneNode) childItem3);
      MusicManager.Play("music/gameplay", true);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.audioManager.AddSound("heroshoot", "sounds/heroshoot");
      this.audioManager.AddSound("eblaster", "sounds/eblaster");
      this.audioManager.AddSound("iontachyon", "sounds/iontachyon");
      this.audioManager.AddSound("explosion", "sounds/explosion");
      this.audioManager.AddSound("explosion2", "sounds/explosion_new");
      this.audioManager.AddSound("newlevel", "sounds/newlevel");
      this.audioManager.AddSound("sputnik", "sounds/sputnik");
      this.audioManager.AddSound("oneup", "sounds/oneup");
      this.audioManager.AddSound("asthit1", "sounds/asthit1");
      this.audioManager.AddSound("asthit2", "sounds/asthit2");
      this.audioManager.AddSound("cancel", "sounds/cancel");
      this.audioManager.AddSound("collected", "sounds/collected");
    }

    public override void OnScreenLoaded()
    {
      if (Globals.saveManager.saveState.Level != 4)
        return;
      Engine.isPaused = true;
      this.screenManager.AddScreen((GameScreen) new ASTHelpScreen());
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      if (Engine.isPaused || Engine.isObscured)
        return;
      Vector2 vector2 = this.playLayer.heroSprite.Position.ToVector2();
      vector2.X = MathHelper.Clamp(vector2.X, 240f, 4760f);
      vector2.Y = MathHelper.Clamp(vector2.Y, 400f, 4600f);
      this.theCamera.Position = vector2;
      this.starfield.Camera.Position = this.theCamera.Position * 0.8f;
      this.stars.Opacity = Tools.SineAnimation(gameTime, 1f, 0.5f, 1f);
      this.borderOpacity = Tools.SineAnimation(gameTime, 10f, 0.0f, 1f);
      this.blON.Opacity = this.borderOpacity;
      this.brON.Opacity = this.borderOpacity;
      this.bbON.Opacity = this.borderOpacity;
      this.btON.Opacity = this.borderOpacity;
      if (this.Win)
      {
        this.victoryTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ((double) this.victoryTimer >= 2.0)
        {
          this.Win = false;
          LoadingScreen.Load(1f, "loading", 
              (GameScreen) new InvadersScreen(true),
              (GameScreen) new PauseScreen());
        }
      }
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void HandleInput(GestureSample gesture)
    {
      if (this.BackKeyPressed() && !this.gameOver && !this.IsExiting)
      {
        State st = new State()
        {
          Lives = this.uiNode.Lives,
          Score = this.uiNode.scoreNumber.Number,
          Level = Globals.saveManager.saveState.Level
        };
        Engine.isPaused = true;
        this.GetScreenByType<ASTPauseScreen>().Show(st);
      }
      base.HandleInput(gesture);
    }

    public void ResetGame()
    {
    }

    public void NotifyGameOver(bool win)
    {
      if (this.gameOver || this.IsExiting)
        return;
      if (win)
      {
        this.Win = true;
        this.playLayer.StopPlay();
        this.uiNode.NotifyVictory();
        this.audioManager.Play("newlevel");
        Globals.saveManager.saveState.AdvanceLevel();
        Globals.saveManager.saveState.Lives = this.uiNode.Lives;
        Globals.saveManager.saveState.Score = this.uiNode.scoreNumber.Number;
        Globals.saveManager.SaveGame();
      }
      else
      {
        this.playLayer.StopPlay();
        this.gameOver = true;
        Globals.saveManager.playerScores.AddHiScore(this.uiNode.scoreNumber.Number);
        Engine.isPaused = true;
        Globals.saveManager.ResetGame(true);
        this.screenManager.AddScreen((GameScreen) new ASTDefeatScreen());
      }
    }

    public override void OnFASActivate()
    {
      if (Engine.isPaused || this.gameOver || this.IsExiting)
        return;
      State st = new State()
      {
        Lives = this.uiNode.Lives,
        Score = this.uiNode.scoreNumber.Number,
        Level = Globals.saveManager.saveState.Level
      };
      Engine.isPaused = true;
      this.GetScreenByType<ASTPauseScreen>().Show(st);
    }

    public override void OnDeactivate()
    {
      Globals.saveManager.saveState.Lives = this.uiNode.Lives;
      Globals.saveManager.saveState.Score = this.uiNode.scoreNumber.Number;
      Globals.saveManager.SaveGame();
    }
  }
}
