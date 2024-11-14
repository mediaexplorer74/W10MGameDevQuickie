// Decompiled with JetBrains decompiler
// Type: Mangopollo.Tiles.CycleTileData
// Assembly: Mangopollo.Light, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 113B3C86-1493-4043-B3A9-B6A17EC8E051
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\Mangopollo.Light.dll

//using Microsoft.Phone.Shell;
using GameManager;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mangopollo.Tiles
{
  public class CycleTileData
  {
    private readonly object _shelltiledata;

    public CycleTileData()
    {
            this._shelltiledata = default;
               // Type.GetType("Microsoft.Phone.Shell.CycleTileData, Microsoft.Phone").GetConstructor(new Type[0]).Invoke((object[]) null);
    }

    public string Title
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (Title));
      set => Utils.SetProperty(this._shelltiledata, nameof (Title), (object) value);
    }

    public int? Count
    {
      get => (int?) Utils.GetProperty(this._shelltiledata, nameof (Count));
      set => Utils.SetProperty(this._shelltiledata, nameof (Count), (object) value);
    }

    public IEnumerable<Uri> CycleImages
    {
      get => (IEnumerable<Uri>) Utils.GetProperty(this._shelltiledata, nameof (CycleImages));
      set => Utils.SetProperty(this._shelltiledata, nameof (CycleImages), (object) value);
    }

    public Uri SmallBackgroundImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (SmallBackgroundImage));
      set => Utils.SetProperty(this._shelltiledata, nameof (SmallBackgroundImage), (object) value);
    }

    public static implicit operator ShellTileData(CycleTileData data)
    {
      return (ShellTileData) data._shelltiledata;
    }

    public ShellTileData ToShellTileData() => (ShellTileData) this._shelltiledata;
  }
}
