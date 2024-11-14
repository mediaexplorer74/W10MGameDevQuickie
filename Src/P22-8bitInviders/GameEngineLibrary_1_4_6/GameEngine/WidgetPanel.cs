// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetPanel
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class WidgetPanel : Widget
  {
    private List<Vector3> originalPositions;

    public WidgetPanel(string sourcename, Vector2 position, string contentname, GameScreen screen)
      : base(sourcename, position, contentname, screen)
    {
      this.originalPositions = new List<Vector3>();
      this.handleInput = true;
      this.SetSourceRect();
    }

    public override void Update(GameTime gameTime, ref Matrix worldTransform)
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
        sceneNode.Position = this.Position + sceneNode.Direction;
      base.Update(gameTime, ref worldTransform);
    }

    public override bool HandleTouch(GestureSample gesture, TouchInput touchInput)
    {
      bool flag = false;
      foreach (SceneNode sceneNode in (List<SceneNode>) this)
      {
        if (sceneNode is Widget && ((Widget) sceneNode).HandleTouch(gesture, touchInput))
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      this.Screen.spriteBatch.Draw(this.tex, this.Position.ToVector2(), new Rectangle?(this.SourceRect), this.color * this.Opacity * this.uiManager.MasterOpacity, MathHelper.ToRadians(this.Rotation), this.Pivot.ToVector2(), this.Scale, this.Facing, 0.0f);
      base.Draw();
    }
  }
}
