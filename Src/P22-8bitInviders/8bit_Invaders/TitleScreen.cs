// Decompiled with JetBrains decompiler
// Type: GameManager.TitleScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input; //!

#nullable disable
namespace GameManager
{
  public class TitleScreen : GameScreen
  {
    private UIManager uiNode;
    private Sprite mainflash;

    public TitleScreen()
    {
      this.TransitionTime(1f, 0.5f);
      this.AddSpriteSheet("sheets/invaders.xml", "sheets/title.xml");
    }

    public override void Initialize()
    {
      string str = "sheets/title";
      Layer2D childItem1 = new Layer2D((GameScreen) this);
      this.uiNode = new UIManager((GameScreen) this);
      this.uiNode.RegisterWidget((Widget) new WidgetButton("newgame", 
          new Vector2(240f, 317f), str, (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("continue", 
          new Vector2(240f, 397f), str, (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("hiscores", 
          new Vector2(240f, 477f), str, (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("options", 
          new Vector2(240f, 557f), str, (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("howto", 
          new Vector2(240f, 631f), str, (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("ratebutton",
          new Vector2(100f, 720f), str, (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("help", 
          new Vector2(407f, 720f), str, (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.FireTapWithName += 
                new UIManager.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      InvadersStarfield childItem2 = 
                new InvadersStarfield((GameScreen) this);

      InvadersBG childItem3 = new InvadersBG((GameScreen) this);

      this.mainflash = new Sprite("titleflash", new Vector2(240f, 205f), str, (GameScreen) this);
      childItem1.Add((SceneNode) childItem3);
      childItem1.Add((SceneNode) new Sprite("maintitle", 
          new Vector2(240f, 205f), str, (GameScreen) this));
      childItem1.Add((SceneNode) this.mainflash);
      childItem1.Add((SceneNode) this.uiNode);
      childItem1.SetSourceRect();
      this.sceneGraph.Add((SceneNode) childItem2);
      this.sceneGraph.Add((SceneNode) childItem1);
      
      //RnD
      MusicManager.Play("music/title2", true);
      //MusicManager.Play("music/Bullet Defender -In Game", true);
      
      if (!Globals.saveManager.saveState.isNewGame)
        return;
      this.uiNode["continue"].Enable(false);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.audioManager.AddSound("click", "sounds/buttonpress");
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      this.mainflash.Opacity = Tools.SineAnimation(gameTime, 30f, 0.0f, 1f);
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    private void uiNode_FireTapWithName(int ID, string btnName)
    {
      this.audioManager.Play("click");
      switch (btnName)
      {
        case "newgame":
          Globals.saveManager.ResetGame(true);
          LoadingScreen.Load(1f, "loading", (GameScreen) new InvadersScreen(false),
              (GameScreen) new PauseScreen());
          break;
        case "continue":
          switch (Globals.saveManager.saveState.Level % 10)
          {
            case 4:
            case 7:
              LoadingScreen.Load(1f, "loading", (GameScreen) new AsteroidScreen(), 
                  (GameScreen) new ASTPauseScreen());
              return;
            default:
              LoadingScreen.Load(1f, "loading", (GameScreen) new InvadersScreen(true),
                  (GameScreen) new PauseScreen());
              return;
          }
        case "hiscores":
          this.uiNode.isDisabled = true;
          this.screenManager.AddScreen((GameScreen) new HiScoresScreen());
          break;
        case "options":
          this.uiNode.isDisabled = true;
          this.screenManager.AddScreen((GameScreen) new OptionsScreen());
          break;
        case "howto":
          this.screenManager.AddScreen((GameScreen) new HowToScreen());
          break;
        case "ratebutton":
          PhoneTasks.RateReviewApp();
          break;
        case "help":
          this.uiNode.isDisabled = true;
          this.screenManager.AddScreen((GameScreen) new HelpScreen());
          break;
      }
    }

    public void EnableUI()
    {
      this.audioManager.Play("click");
      this.uiNode.isDisabled = false;
    }

    public override void HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      if (this.BackKeyPressed())
        Engine.game.Exit();
      base.HandleInput(gesture);
    }
  }
}
