// Decompiled with JetBrains decompiler
// Type: GameEngine.WidgetSprite
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public class WidgetSprite : Widget
  {
    public Color tintColor = Color.White;

    public WidgetSprite(string name, Vector2 position, string contentname, GameScreen screen)
      : base(name, position, contentname, screen)
    {
      this.handleInput = true;
    }

    public override void Draw()
    {
      if (!this.Visible)
        return;
      this.SetTextureSource(this.Name);
      this.DrawWidget(this.tintColor);
    }

    private void DrawWidget(Color color)
    {
      this.Screen.spriteBatch.Draw(this.tex, this.Position.ToVector2(), new Rectangle?(this.SourceRect), color * this.Opacity * this.uiManager.MasterOpacity, MathHelper.ToRadians(this.Rotation), this.Pivot.ToVector2(), this.Scale, this.Facing, 0.0f);
    }
  }
}
