// Decompiled with JetBrains decompiler
// Type: GLEED.PathItem
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GLEED
{
  public class PathItem : Item
  {
    public Vector2[] LocalPoints;
    public Vector2[] WorldPoints;
    public bool IsPolygon;
    public int LineWidth;
    public Color LineColor;
  }
}
