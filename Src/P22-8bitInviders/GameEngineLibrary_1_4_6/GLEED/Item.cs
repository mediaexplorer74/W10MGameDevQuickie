// Decompiled with JetBrains decompiler
// Type: GLEED.Item
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Xml.Serialization;

#nullable disable
namespace GLEED
{
  [XmlInclude(typeof (RectangleItem))]
  [XmlInclude(typeof (PathItem))]
  [XmlInclude(typeof (TextureItem))]
  [XmlInclude(typeof (CircleItem))]
  public class Item
  {
    [XmlAttribute]
    public string Name;
    [XmlAttribute]
    public bool Visible;
    public Vector2 Position;
    public SerializableDictionary CustomProperties;

    public Item() => this.CustomProperties = new SerializableDictionary();
  }
}
