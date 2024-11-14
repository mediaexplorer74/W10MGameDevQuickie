// Decompiled with JetBrains decompiler
// Type: GameManager.InvadersScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class InvadersScreen : GameScreen
  {
    private AccelerometerInput accInput;
    public InvaderHero hero;
    public InvadersFX fxNode;
    public InvadersFXAdditiveLayer fxAddLayer;
    public InvadersCritterFactory critterFactory;
    public InvadersUI uiNode;
    public bool gameOver;
    private bool Rehydrate;
    private Rectangle screenBounds = new Rectangle(0, 0, 480, 800);

    public InvadersScreen(bool rehydrate)
    {
      this.Rehydrate = rehydrate;
      this.TransitionTime(1f, 0.5f);
      this.AddSpriteSheet("sheets/invaders.xml");
    }

    public override void Initialize()
    {
      this.accInput = new AccelerometerInput();
      Layer2D childItem1 = new Layer2D((GameScreen) this);
      InvadersStarfield childItem2 = new InvadersStarfield((GameScreen) this);
      InvadersBG childItem3 = new InvadersBG((GameScreen) this);
      this.hero = new InvaderHero((GameScreen) this);
      this.critterFactory = new InvadersCritterFactory((GameScreen) this);
      this.fxNode = new InvadersFX((GameScreen) this);
      this.fxAddLayer = new InvadersFXAdditiveLayer((GameScreen) this);
      this.uiNode = new InvadersUI((GameScreen) this, this.Rehydrate);
      childItem1.Add((SceneNode) childItem3);
      childItem1.Add((SceneNode) this.hero);
      childItem1.Add((SceneNode) this.critterFactory);
      childItem1.Add((SceneNode) this.fxNode);
      childItem1.Add((SceneNode) this.uiNode);
      this.sceneGraph.Add((SceneNode) childItem2);
      this.sceneGraph.Add((SceneNode) childItem1);
      this.sceneGraph.Add((SceneNode) this.fxAddLayer);
      this.accInput.Start();
      this.critterFactory.Reset(Globals.saveManager.saveState.Level);
      switch (Globals.saveManager.saveState.Level % 10)
      {
        case 0:
        case 3:
          MusicManager.Play("music/boss", true);
          this.critterFactory.SpawnBoss();
          break;
        default:
          MusicManager.Play("music/gameplay", true);
          break;
      }
      this.fxNode.Reset();
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
      this.audioManager.AddSound("mothership", "sounds/mothership");
      this.audioManager.AddSound("oneup", "sounds/oneup");
      this.audioManager.AddSound("stoptime", "sounds/stoptime");
    }

    public override void OnScreenLoaded()
    {
      this.critterFactory.startGame = true;
      this.uiNode.NotifyNewLevel();
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
        this.GetScreenByType<PauseScreen>().Show(st);
      }
      this.fxNode.isHeroShooting = Globals.saveManager.optionsState.autoShoot 
                || Engine.touchInputManager[1].HeldInRectangle(new Rectangle(0, 0, 480, 800))
                || Engine.touchInputManager[2].HeldInRectangle(new Rectangle(0, 0, 480, 800));
      base.HandleInput(gesture);
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      if (Engine.isPaused || Engine.isObscured)
        return;
      if (!Globals.saveManager.optionsState.usingTouch)
      {
        if (Tools.WithinTolerance((float) this.accInput.X, 0.0f, 0.2f))
          this.hero.CenterStick();
        else if (this.accInput.X < -0.10000000149011612)
          this.hero.MoveLeft((float) this.accInput.X);
        else if (this.accInput.X > 0.10000000149011612)
          this.hero.MoveRight((float) this.accInput.X);
      }
      else if (Engine.touchInputManager[1].HeldInRectangle(this.screenBounds))
        this.hero.SetXTouchPosition(Engine.touchInputManager[1].touchPosition.X);
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public void NotifyGameOver(bool win)
    {
      if (this.gameOver && !this.IsExiting)
        return;
      if (win)
      {
        Globals.saveManager.saveState.AdvanceLevel();
        this.uiNode.NotifyNewLevel();
        switch (Globals.saveManager.saveState.Level % 10)
        {
          case 0:
          case 3:
            this.critterFactory.SpawnBoss();
            this.fxNode.Reset();
            break;
          case 1:
          case 2:
          case 5:
          case 6:
          case 8:
          case 9:
            this.critterFactory.Reset(Globals.saveManager.saveState.Level);
            this.fxNode.Reset();
            break;
          case 4:
          case 7:
            this.gameOver = true;
            Globals.saveManager.saveState.Lives = this.uiNode.Lives;
            Globals.saveManager.saveState.Score = this.uiNode.scoreNumber.Number;
            Globals.saveManager.SaveGame();
            LoadingScreen.Load(1f, "loading", (GameScreen) new AsteroidScreen(), (GameScreen) new ASTPauseScreen());
            break;
        }
      }
      else
      {
        this.gameOver = true;
        Globals.saveManager.playerScores.AddHiScore(this.uiNode.scoreNumber.Number);
        Engine.isPaused = true;
        Globals.saveManager.ResetGame(true);
        this.screenManager.AddScreen((GameScreen) new DefeatScreen());
      }
    }

    public void ResetGame()
    {
      if (MusicManager.currentlyPlaying.Contains("boss"))
        MusicManager.Play("music/gameplay", true);
      this.gameOver = false;
      this.hero.Reset();
      this.hero.hero.Position.X = 240f;
      this.uiNode.Reset();
      this.critterFactory.Reset(Globals.saveManager.saveState.Level);
      this.fxNode.Reset();
      this.uiNode.NotifyNewLevel();
    }

    public override void OnScreenExiting() => this.accInput.Stop();

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
      this.GetScreenByType<PauseScreen>().Show(st);
    }

    public override void OnDeactivate()
    {
      Globals.saveManager.saveState.Lives = this.uiNode.Lives;
      Globals.saveManager.saveState.Score = this.uiNode.scoreNumber.Number;
      Globals.saveManager.SaveGame();
    }
  }
}
