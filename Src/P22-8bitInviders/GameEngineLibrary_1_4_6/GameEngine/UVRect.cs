// Decompiled with JetBrains decompiler
// Type: GameEngine.UVRect
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public struct UVRect
  {
    public Vector2 uv1;
    public Vector2 uv2;
    public Vector2 uv3;
    public Vector2 uv4;

    public UVRect(Vector2 tx1, Vector2 tx2, Vector2 tx3, Vector2 tx4)
    {
      this.uv1 = tx1;
      this.uv2 = tx2;
      this.uv3 = tx3;
      this.uv4 = tx4;
    }
  }
}
