// Decompiled with JetBrains decompiler
// Type: Mangopollo.Tiles.TilesCreator
// Assembly: Mangopollo.Light, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 113B3C86-1493-4043-B3A9-B6A17EC8E051
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\Mangopollo.Light.dll

//using Microsoft.Phone.Shell;
using GameManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
//using System.Windows.Media;

#nullable disable
namespace Mangopollo.Tiles
{
  public static class TilesCreator
  {
    public static ShellTileData CreateFromStandardTile(
      StandardTileData standardtiledata,
      Uri smallBackgroundImage)
    {
      return TilesCreator.CreateFlipTile(((ShellTileData) standardtiledata).Title, 
          standardtiledata.BackTitle, standardtiledata.BackContent, (string) null, 
          standardtiledata.Count, smallBackgroundImage, standardtiledata.BackgroundImage, 
          standardtiledata.BackBackgroundImage, (Uri) null, (Uri) null);
    }

    public static ShellTileData CreateFromStandardTile(
      StandardTileData standardtiledata,
      Uri smallBackgroundImage,
      string wideBackContent,
      Uri wideBackgroundImage,
      Uri wideBackBackgroundImage)
    {
      return TilesCreator.CreateFlipTile(((ShellTileData) standardtiledata).Title, 
          standardtiledata.BackTitle, standardtiledata.BackContent, 
          wideBackContent, standardtiledata.Count, smallBackgroundImage, 
          standardtiledata.BackgroundImage, standardtiledata.BackBackgroundImage, 
          wideBackgroundImage, wideBackBackgroundImage);
    }

    public static ShellTileData CreateFlipTile(
      string title,
      string backTitle,
      string backContent,
      int? count,
      Uri smallBackgroundImage,
      Uri backgroundImage,
      Uri backBackgroundImage)
    {
      return TilesCreator.CreateFlipTile(title, backTitle, backContent,
          (string) null, count, smallBackgroundImage, backgroundImage,
          backBackgroundImage, (Uri) null, (Uri) null);
    }

    public static ShellTileData CreateFlipTile(
      string title,
      string backTitle,
      string backContent,
      string wideBackContent,
      int? count,
      Uri smallBackgroundImage,
      Uri backgroundImage,
      Uri backBackgroundImage,
      Uri wideBackgroundImage,
      Uri wideBackBackgroundImage)
    {
      object instance = default;
              //  Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone")
              //  .GetConstructor(new Type[0]).Invoke((object[]) null);
      Utils.SetProperty(instance, "Title", (object) title);
      Utils.SetProperty(instance, "Count", (object) count);
      Utils.SetProperty(instance, "BackTitle", (object) backTitle);
      Utils.SetProperty(instance, "BackContent", (object) backContent);
      Utils.SetProperty(instance, "SmallBackgroundImage", (object) smallBackgroundImage);
      Utils.SetProperty(instance, "BackgroundImage", (object) backgroundImage);
      Utils.SetProperty(instance, "BackBackgroundImage", (object) backBackgroundImage);
      Utils.SetProperty(instance, "WideBackgroundImage", (object) wideBackgroundImage);
      Utils.SetProperty(instance, "WideBackBackgroundImage", (object) wideBackBackgroundImage);
      Utils.SetProperty(instance, "WideBackContent", (object) wideBackContent);
      return (ShellTileData) instance;
    }

    public static ShellTileData CreateIconicTile(
      string title,
      int? count,
      Color backgroundColor,
      Uri icon,
      Uri smallIcon)
    {
      return TilesCreator.CreateIconicTile(title, count, 
          backgroundColor, icon, smallIcon, (string) null, (string) null, (string) null);
    }

    public static ShellTileData CreateIconicTile(
      string title,
      int? count,
      Color backgroundColor,
      Uri icon,
      Uri smallIcon,
      string wideTitle,
      string wideLine1,
      string wideLine2)
    {
      object instance = default;
            //Type.GetType("Microsoft.Phone.Shell.IconicTileData, Microsoft.Phone")
            //.GetConstructor(new Type[0]).Invoke((object[]) null);
      Utils.SetProperty(instance, "Title", (object) title);
      Utils.SetProperty(instance, "Count", (object) count);
      Utils.SetProperty(instance, "BackgroundColor", (object) backgroundColor);
      Utils.SetProperty(instance, "IconImage", (object) icon);
      Utils.SetProperty(instance, "SmallIconImage", (object) smallIcon);
      Utils.SetProperty(instance, "WideContent1", (object) wideTitle);
      Utils.SetProperty(instance, "WideContent2", (object) wideLine1);
      Utils.SetProperty(instance, "WideContent3", (object) wideLine2);
      return (ShellTileData) instance;
    }

    public static ShellTileData CreateCyclicTile(
      string title,
      int? count,
      Uri smallbackground,
      IEnumerable<Uri> images)
    {
            object instance = default;
            //Type.GetType("Microsoft.Phone.Shell.CycleTileData, Microsoft.Phone")
            //.GetConstructor(new Type[0]).Invoke((object[]) null);
      Utils.SetProperty(instance, "Title", (object) title);
      Utils.SetProperty(instance, "Count", (object) count);
      Utils.SetProperty(instance, "SmallBackgroundImage", (object) smallbackground);
      Utils.SetProperty(instance, "CycleImages", (object) images.ToArray<Uri>());
      return (ShellTileData) instance;
    }
  }
}
