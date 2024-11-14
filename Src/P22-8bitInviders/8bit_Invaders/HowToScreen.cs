// Decompiled with JetBrains decompiler
// Type: GameManager.HowToScreen
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

#nullable disable
namespace GameManager
{
  public class HowToScreen : GameScreen
  {
    public HowToScreen()
    {
      this.TransitionTime(0.0f, 0.0f);
      this.blockInput = true;
      this.AddSpriteSheet("sheets/title.xml");
    }

    public override void Initialize()
    {
      Layer2D childItem = new Layer2D((GameScreen) this);
      childItem.Add((SceneNode) new JPGSprite("howto", new Vector2(240f, 400f), "sheets/howto", (GameScreen) this));
      this.sceneGraph.Add((SceneNode) childItem);
    }

    public override void HandleInput(GestureSample gesture)
    {
      if (this.BackKeyPressed())
      {
        this.GetScreenByType<TitleScreen>().EnableUI();
        this.ExitScreen();
      }
      base.HandleInput(gesture);
    }
  }
}
