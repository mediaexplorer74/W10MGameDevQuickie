// Decompiled with JetBrains decompiler
// Type: GameManager.HiScoresScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class HiScoresScreen : GameScreen
  {
    private UIManager uiNode;
    private BitmapNumbers[] scores;

    public HiScoresScreen()
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
      this.uiNode.RegisterWidget((Widget) new WidgetButton("resetbutton", new Vector2(240f, 720f), "sheets/title", (GameScreen) this)
      {
        pressedColor = Color.Red
      });
      childItem.Add((SceneNode) this.uiNode);
      this.scores = new BitmapNumbers[10];
      int y = 350;
      for (int index = 0; index < 10; ++index)
      {
        this.scores[index] = new BitmapNumbers("n", new Vector2(240f, (float) y), (GameScreen) this, "sheets/title");
        this.scores[index].Number = Globals.saveManager.playerScores.Scores[9 - index];
        this.scores[index].HorizontalSqueaze = -5;
        childItem.Add((SceneNode) this.scores[index]);
        y += 30;
      }
      childItem.SetSourceRect();
      for (int index = 0; index < 10; ++index)
        this.scores[index].numberAlignment = Alignment.Center;
      this.sceneGraph.Add((SceneNode) childItem);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.audioManager.AddSound("click", "sounds/buttonpress");
    }

    private void uiNode_FireTapWithName(int ID, string btnName)
    {
      if (!(btnName == "resetbutton"))
        return;
      this.audioManager.Play("click");
      Globals.saveManager.ResetScores();
      this.uiNode[0].Enable(false);
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
