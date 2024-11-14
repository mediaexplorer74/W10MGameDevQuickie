// Decompiled with JetBrains decompiler
// Type: GameManager.HelpScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class HelpScreen : GameScreen
  {
    private UIManager uiNode;

    public HelpScreen()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.blockInput = true;
      this.AddSpriteSheet("sheets/title.xml");
    }

    public override void Initialize()
    {
      Layer2D childItem = new Layer2D((GameScreen) this);
      this.uiNode = new UIManager((GameScreen) this);
      this.uiNode.FireTapWithName += new UIManager.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      this.uiNode.RegisterWidget((Widget) new WidgetButton("report", new Vector2(240f, 344f), "sheets/title", (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("twitter", new Vector2(240f, 464f), "sheets/title", (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetButton("web", new Vector2(240f, 584f), "sheets/title", (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      this.uiNode.RegisterWidget((Widget) new WidgetSprite("music", new Vector2(240f, 704f), "sheets/title", (GameScreen) this));
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
        case "report":
          PhoneTasks.Email("8bit Invaders [" + Tools.GetAppVersion() + "]");
          break;
        case "twitter":
          PhoneTasks.WebBrowser("http://twitter.com/glowpuff");
          break;
        case "web":
          PhoneTasks.WebBrowser("http://www.glowpuff.com");
          break;
      }
    }

    public override void HandleInput(GestureSample gesture)
    {
      this.uiNode.HandleTouch(gesture);
      if (this.BackKeyPressed())
      {
        this.GetScreenByType<TitleScreen>().EnableUI();
        this.ExitScreen();
      }
      base.HandleInput(gesture);
    }
  }
}
