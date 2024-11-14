// Decompiled with JetBrains decompiler
// Type: GLEED.CustomProperty
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using System;

#nullable disable
namespace GLEED
{
  public class CustomProperty
  {
    public string name;
    public object value;
    public Type type;
    public string description;

    public CustomProperty()
    {
    }

    public CustomProperty(string n, object v, Type t, string d)
    {
      this.name = n;
      this.value = v;
      this.type = t;
      this.description = d;
    }

    public CustomProperty clone()
    {
      return new CustomProperty(this.name, this.value, this.type, this.description);
    }
  }
}
