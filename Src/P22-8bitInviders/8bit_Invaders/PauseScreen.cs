// Decompiled with JetBrains decompiler
// Type: GameManager.PauseScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class PauseScreen : GameScreen
  {
    private UIManager uiNode;
    private State state;

    public PauseScreen()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.blockInput = true;
    }

    public override void Initialize()
    {
      Layer2D childItem = new Layer2D((GameScreen) this);
      childItem.Add((SceneNode) new Sprite("bg", new Vector2(240f, 450f), "sheets/pausescreen", (GameScreen) this));
      this.uiNode = new UIManager((GameScreen) this);
      this.uiNode.FireTapWithName += new UIManager.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      this.uiNode.RegisterWidget((Widget) new WidgetRectangle("savequit", new Rectangle(60, 420, 360, 60), (GameScreen) this)
      {
        ShowChrome = false,
        ShowWhenInteracted = false
      });
      this.uiNode.RegisterWidget((Widget) new WidgetRectangle("options", new Rectangle(60, 500, 360, 60), (GameScreen) this)
      {
        ShowChrome = false,
        ShowWhenInteracted = false
      });
      childItem.Add((SceneNode) this.uiNode);
      childItem.SetSourceRect();
      this.sceneGraph.Add((SceneNode) childItem);
      this.isDisabled = true;
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
        case "savequit":
          Globals.saveManager.saveState.Lives = this.state.Lives;
          Globals.saveManager.saveState.Level = this.state.Level;
          Globals.saveManager.saveState.Score = this.state.Score;
          Globals.saveManager.SaveGame();
          Engine.isPaused = false;
          LoadingScreen.Load(1f, "loading", (GameScreen) new TitleScreen());
          break;
        case "options":
          this.screenManager.AddScreen((GameScreen) new IngameOptionsScreen());
          break;
      }
    }

    public override void HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      if (this.BackKeyPressed())
      {
        this.audioManager.Play("click");
        this.isDisabled = true;
        Engine.isPaused = false;
      }
      base.HandleInput(gesture);
    }

    public void Show(State st)
    {
      this.audioManager.Play("click");
      this.state = st;
      this.isDisabled = false;
    }
  }
}
