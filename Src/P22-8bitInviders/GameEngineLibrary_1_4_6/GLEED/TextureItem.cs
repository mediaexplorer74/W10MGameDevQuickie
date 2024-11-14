// Decompiled with JetBrains decompiler
// Type: GLEED.TextureItem
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GLEED
{
  public class TextureItem : Item
  {
    public float Rotation;
    public Vector2 Scale;
    public Color TintColor;
    public bool FlipHorizontally;
    public bool FlipVertically;
    public string texture_filename;
    public string asset_name;
    public Vector2 Origin;
  }
}
