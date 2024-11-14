// Decompiled with JetBrains decompiler
// Type: GameManager.ASTDefeatScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class ASTDefeatScreen : GameScreen
  {
    private UIManager uiNode;

    public ASTDefeatScreen()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.blockInput = true;
    }

    public override void Initialize()
    {
      Layer2D childItem = new Layer2D((GameScreen) this);
      childItem.Add((SceneNode) new Sprite("bg", new Vector2(240f, 450f), "sheets/defeatscreen", (GameScreen) this));
      this.uiNode = new UIManager((GameScreen) this);
      this.uiNode.FireTapWithName += new UIManager.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      this.uiNode.RegisterWidget((Widget) new WidgetRectangle("playagain", new Rectangle(100, 420, 280, 60), (GameScreen) this)
      {
        ShowChrome = false,
        ShowWhenInteracted = false
      });
      this.uiNode.RegisterWidget((Widget) new WidgetRectangle("continue", new Rectangle(80, 490, 322, 60), (GameScreen) this)
      {
        ShowChrome = false,
        ShowWhenInteracted = false
      });
      childItem.Add((SceneNode) this.uiNode);
      childItem.SetSourceRect();
      this.sceneGraph.Add((SceneNode) childItem);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.audioManager.AddSound("click", "sounds/buttonpress");
    }

    private void uiNode_FireTapWithName(int ID, string btnName)
    {
      this.audioManager.Play("click");
      switch (btnName)
      {
        case "continue":
          Engine.isPaused = false;
          this.ExitScreen();
          LoadingScreen.Load(1f, "loading", (GameScreen) new TitleScreen());
          break;
        case "playagain":
          Engine.isPaused = false;
          Globals.saveManager.ResetGame(true);
          LoadingScreen.Load(1f, "loading", (GameScreen) new InvadersScreen(false), (GameScreen) new PauseScreen());
          break;
      }
    }

    public override void HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      if (this.BackKeyPressed())
      {
        this.audioManager.Play("click");
        Engine.isPaused = false;
        this.ExitScreen();
        LoadingScreen.Load(1f, "loading", (GameScreen) new TitleScreen());
      }
      base.HandleInput(gesture);
    }
  }
}
