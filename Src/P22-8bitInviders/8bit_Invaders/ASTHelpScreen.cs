// Decompiled with JetBrains decompiler
// Type: GameManager.ASTHelpScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class ASTHelpScreen : GameScreen
  {
    private UIManagerMultiTouch uiNode;

    public ASTHelpScreen()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.removeAnimsOnExit = false;
      this.blockInput = true;
      this.handleInput = true;
    }

    public override void Initialize()
    {
      Layer2D childItem = new Layer2D((GameScreen) this);
      childItem.Add((SceneNode) new JPGSprite("asthelp", new Vector2(240f, 400f), "sheets/asthelp", (GameScreen) this));
      this.uiNode = new UIManagerMultiTouch((GameScreen) this);
      this.uiNode.FireTapWithName += new UIManagerMultiTouch.OnTapBWithNameHandler(this.uiNode_FireTapWithName);
      this.uiNode.RegisterWidget((Widget) new WidgetButton("ok", new Vector2(59f, 726f), "sheets/ok", (GameScreen) this));
      childItem.Add((SceneNode) this.uiNode);
      childItem.SetSourceRect();
      this.sceneGraph.Add((SceneNode) childItem);
    }

    private void uiNode_FireTapWithName(int ID, string btnName)
    {
      if (ID != 0)
        return;
      Engine.isPaused = false;
      this.ExitScreen();
    }

    public override void Update(
      GameTime gameTime,
      bool otherScreenHasFocus,
      bool coveredByOtherScreen)
    {
      this.uiNode[0].Scale = Tools.SineAnimation(gameTime, 20f, 0.95f, 1.05f);
      base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void HandleInput(GestureSample gesture)
    {
      if (this.BackKeyPressed())
      {
        Engine.isPaused = false;
        this.ExitScreen();
      }
      this.uiNode.HandleTouch(gesture);
      base.HandleInput(gesture);
    }
  }
}
