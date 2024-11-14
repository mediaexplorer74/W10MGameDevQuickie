// Decompiled with JetBrains decompiler
// Type: GameEngine.PolygonCollisionResult
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public struct PolygonCollisionResult
  {
    public bool WillIntersect;
    public bool Intersect;
    public Vector2 MinimumTranslationVector2;
  }
}
