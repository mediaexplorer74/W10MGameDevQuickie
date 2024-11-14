// Decompiled with JetBrains decompiler
// Type: GLEED.Layer
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace GLEED
{
  public class Layer
  {
    [XmlAttribute]
    public string Name;
    [XmlAttribute]
    public bool Visible;
    public List<Item> Items;
    public Vector2 ScrollSpeed;

    public Layer()
    {
      this.Items = new List<Item>();
      this.ScrollSpeed = Vector2.One;
    }
  }
}
