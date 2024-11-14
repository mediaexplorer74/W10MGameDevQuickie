// Decompiled with JetBrains decompiler
// Type: Mangopollo.Tiles.FlipTileData
// Assembly: Mangopollo.Light, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 113B3C86-1493-4043-B3A9-B6A17EC8E051
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\Mangopollo.Light.dll

//using Microsoft.Phone.Shell;
using GameManager;
using System;

#nullable disable
namespace Mangopollo.Tiles
{
  public class FlipTileData
  {
    private readonly object _shelltiledata;

    public FlipTileData()
    {
       this._shelltiledata = default;//Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone").GetConstructor(new Type[0]).Invoke((object[]) null);
    }

    public string Title
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (Title));
      set => Utils.SetProperty(this._shelltiledata, nameof (Title), (object) value);
    }

    public Uri BackBackgroundImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (BackBackgroundImage));
      set => Utils.SetProperty(this._shelltiledata, nameof (BackBackgroundImage), (object) value);
    }

    public string BackContent
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (BackContent));
      set => Utils.SetProperty(this._shelltiledata, nameof (BackContent), (object) value);
    }

    public Uri BackgroundImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (BackgroundImage));
      set => Utils.SetProperty(this._shelltiledata, nameof (BackgroundImage), (object) value);
    }

    public string BackTitle
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (BackTitle));
      set => Utils.SetProperty(this._shelltiledata, nameof (BackTitle), (object) value);
    }

    public int? Count
    {
      get => (int?) Utils.GetProperty(this._shelltiledata, nameof (Count));
      set => Utils.SetProperty(this._shelltiledata, nameof (Count), (object) value);
    }

    public Uri SmallBackgroundImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (SmallBackgroundImage));
      set => Utils.SetProperty(this._shelltiledata, nameof (SmallBackgroundImage), (object) value);
    }

    public Uri WideBackBackgroundImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (WideBackBackgroundImage));
      set
      {
        Utils.SetProperty(this._shelltiledata, nameof (WideBackBackgroundImage), (object) value);
      }
    }

    public string WideBackContent
    {
      get => (string) Utils.GetProperty(this._shelltiledata, nameof (WideBackContent));
      set => Utils.SetProperty(this._shelltiledata, nameof (WideBackContent), (object) value);
    }

    public Uri WideBackgroundImage
    {
      get => (Uri) Utils.GetProperty(this._shelltiledata, nameof (WideBackgroundImage));
      set => Utils.SetProperty(this._shelltiledata, nameof (WideBackgroundImage), (object) value);
    }

    public static implicit operator ShellTileData(FlipTileData data)
    {
      return (ShellTileData) data._shelltiledata;
    }

    public ShellTileData ToShellTileData()
    {
        return (ShellTileData)this._shelltiledata;
    }
  }
}
