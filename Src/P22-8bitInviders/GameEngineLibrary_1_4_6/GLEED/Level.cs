// Decompiled with JetBrains decompiler
// Type: GLEED.Level
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

#nullable disable
namespace GLEED
{
  public class Level
  {
    [XmlAttribute]
    public string Name;
    [XmlAttribute]
    public bool Visible;
    public List<Layer> Layers;
    public SerializableDictionary CustomProperties;

    public Level()
    {
      this.Visible = true;
      this.Layers = new List<Layer>();
      this.CustomProperties = new SerializableDictionary();
    }

    public static Level FromFile(string filename)
    {
      Level level = (Level) null;
      using (Stream stream = TitleContainer.OpenStream(filename))
        level = (Level) new XmlSerializer(typeof (Level)).Deserialize(stream);
      if (level != null)
      {
        foreach (Layer layer in level.Layers)
        {
          foreach (Item obj in layer.Items)
            obj.CustomProperties.RestoreItemAssociations(level);
        }
      }
      return level;
    }

    public Item getItemByName(string name)
    {
      foreach (Layer layer in this.Layers)
      {
        foreach (Item itemByName in layer.Items)
        {
          if (itemByName.Name == name)
            return itemByName;
        }
      }
      return (Item) null;
    }

    public Layer getLayerByName(string name)
    {
      foreach (Layer layer in this.Layers)
      {
        if (layer.Name == name)
          return layer;
      }
      return (Layer) null;
    }
  }
}
