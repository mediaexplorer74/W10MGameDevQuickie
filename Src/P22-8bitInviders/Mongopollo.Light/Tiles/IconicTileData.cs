// Decompiled with JetBrains decompiler
// Type: Mangopollo.Tiles.IconicTileData
// Assembly: Mangopollo.Light, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 113B3C86-1493-4043-B3A9-B6A17EC8E051
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\Mangopollo.Light.dll

//using Microsoft.Phone.Shell;
using GameManager;
using System;
using Windows.UI;
//using System.Windows.Media;

#nullable disable
namespace Mangopollo.Tiles
{
  public class IconicTileData
  {
    private readonly object _shelltiledata;

    public IconicTileData()
    {
      this._shelltiledata = default;//Type.GetType("Microsoft.Phone.Shell.IconicTileData, Microsoft.Phone").GetConstructor(new Type[0]).Invoke((object[]) null);
    }

    public string Title
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (Title));
      set => Utils.SetProperty(this._shelltiledata, nameof (Title), (object) value);
    }

    public Color BackgroundColor
    {
      get => (Color) Utils.GetProperty(this._shelltiledata, nameof (BackgroundColor));
      set => Utils.SetProperty(this._shelltiledata, nameof (BackgroundColor), (object) value);
    }

    public int? Count
    {
      get => (int?) Utils.GetProperty(this._shelltiledata, nameof (Count));
      set => Utils.SetProperty(this._shelltiledata, nameof (Count), (object) value);
    }

    public Uri IconImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (IconImage));
      set => Utils.SetProperty(this._shelltiledata, nameof (IconImage), (object) value);
    }

    public Uri SmallIconImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (SmallIconImage));
      set => Utils.SetProperty(this._shelltiledata, nameof (SmallIconImage), (object) value);
    }

    public string WideContent1
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (WideContent1));
      set => Utils.SetProperty(this._shelltiledata, nameof (WideContent1), (object) value);
    }

    public string WideContent2
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (WideContent2));
      set => Utils.SetProperty(this._shelltiledata, nameof (WideContent2), (object) value);
    }

    public string WideContent3
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (WideContent3));
      set => Utils.SetProperty(this._shelltiledata, nameof (WideContent3), (object) value);
    }

    public static implicit operator ShellTileData(IconicTileData data)
    {
      return (ShellTileData) data._shelltiledata;
    }

    public ShellTileData ToShellTileData() => (ShellTileData) this._shelltiledata;
  }
}
