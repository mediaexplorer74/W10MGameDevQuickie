// Decompiled with JetBrains decompiler
// Type: GameEngine.PauseOverlay
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#nullable disable
namespace GameEngine
{
  public static class PauseOverlay
  {
    public static Texture2D frozen;

    public static void Draw(SpriteBatch sb, int alpha)
    {
      sb.Begin();
      if (PauseOverlay.frozen != null)
        sb.Draw(PauseOverlay.frozen, Vector2.Zero, Color.White);
      sb.End();
    }
  }
}
