// Decompiled with JetBrains decompiler
// Type: GLEED.SerializableDictionary
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#nullable disable
namespace GLEED
{
  public class SerializableDictionary : Dictionary<string, CustomProperty>, IXmlSerializable
  {
    public SerializableDictionary()
    {
    }

    public SerializableDictionary(SerializableDictionary copyfrom)
      : base((IDictionary<string, CustomProperty>) copyfrom)
    {
      string[] array = new string[this.Keys.Count];
      this.Keys.CopyTo(array, 0);
      foreach (string key in array)
        this[key] = this[key].clone();
    }

    public XmlSchema GetSchema() => (XmlSchema) null;

    public void ReadXml(XmlReader reader)
    {
      bool isEmptyElement = reader.IsEmptyElement;
      reader.Read();
      if (isEmptyElement)
        return;
      while (reader.NodeType != XmlNodeType.EndElement)
      {
        CustomProperty customProperty = new CustomProperty();
        customProperty.name = reader.GetAttribute("Name");
        customProperty.description = reader.GetAttribute("Description");
        string attribute = reader.GetAttribute("Type");
        if (attribute == "string")
          customProperty.type = typeof (string);
        if (attribute == "bool")
          customProperty.type = typeof (bool);
        if (attribute == "Vector2")
          customProperty.type = typeof (Vector2);
        if (attribute == "Color")
          customProperty.type = typeof (Color);
        if (attribute == "Item")
          customProperty.type = typeof (GLEED.Item);
        if ((object) customProperty.type == (object) typeof (GLEED.Item))
        {
          customProperty.value = (object) reader.ReadInnerXml();
          this.Add(customProperty.name, customProperty);
        }
        else
        {
          reader.ReadStartElement("Property");
          object obj = new XmlSerializer(customProperty.type).Deserialize(reader);
          customProperty.value = Convert.ChangeType(obj, customProperty.type, (IFormatProvider) CultureInfo.InvariantCulture);
          this.Add(customProperty.name, customProperty);
          reader.ReadEndElement();
        }
        int content = (int) reader.MoveToContent();
      }
      reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
      foreach (string key in this.Keys)
      {
        writer.WriteStartElement("Property");
        writer.WriteAttributeString("Name", this[key].name);
        if ((object) this[key].type == (object) typeof (string))
          writer.WriteAttributeString("Type", "string");
        if ((object) this[key].type == (object) typeof (bool))
          writer.WriteAttributeString("Type", "bool");
        if ((object) this[key].type == (object) typeof (Vector2))
          writer.WriteAttributeString("Type", "Vector2");
        if ((object) this[key].type == (object) typeof (Color))
          writer.WriteAttributeString("Type", "Color");
        if ((object) this[key].type == (object) typeof (GLEED.Item))
          writer.WriteAttributeString("Type", "Item");
        writer.WriteAttributeString("Description", this[key].description);
        if ((object) this[key].type == (object) typeof (GLEED.Item))
        {
          GLEED.Item obj = (GLEED.Item) this[key].value;
          if (obj != null)
            writer.WriteString(obj.Name);
          else
            writer.WriteString("$null$");
        }
        else
          new XmlSerializer(this[key].type).Serialize(writer, this[key].value);
        writer.WriteEndElement();
      }
    }

    public void RestoreItemAssociations(Level level)
    {
      foreach (CustomProperty customProperty in this.Values)
      {
        if ((object) customProperty.type == (object) typeof (GLEED.Item))
          customProperty.value = (object) level.getItemByName((string) customProperty.value);
      }
    }
  }
}
