// Decompiled with JetBrains decompiler
// Type: GameManager.ConfirmMusic
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class ConfirmMusic : GameScreen
  {
    private UIManager uiNode;

    public ConfirmMusic()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.blockInput = true;
    }

    public override void Initialize()
    {
      Layer2D childItem = new Layer2D((GameScreen) this);
      childItem.Add((SceneNode) new Sprite("bg", new Vector2(240f, 450f), "sheets/confirm", 
          (GameScreen) this));
      this.uiNode = new UIManager((GameScreen) this);
      this.uiNode.FireTapWithName += new UIManager.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      this.uiNode.RegisterWidget((Widget) new WidgetRectangle("yes", new Rectangle(44, 517, 160, 60),
          (GameScreen) this)
      {
        ShowChrome = false,
        ShowWhenInteracted = false
      });
      this.uiNode.RegisterWidget((Widget) new WidgetRectangle("no", new Rectangle(294, 517, 160, 60),
          (GameScreen) this)
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
        case "yes":
          MusicManager.AllowEnableMusic();
          MusicManager.shouldConfirmMusic = false;
          MusicManager.Play("music/title2", true);
          this.GetScreenByType<OptionsScreen>().CheckMusic(true);
          this.ExitScreen();
          break;
        case "no":
          this.GetScreenByType<OptionsScreen>().CheckMusic(false);
          this.ExitScreen();
          break;
      }
    }

    public override void HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      if (this.BackKeyPressed())
      {
        this.GetScreenByType<OptionsScreen>().CheckMusic(false);
        this.ExitScreen();
      }
      base.HandleInput(gesture);
    }
  }
}
