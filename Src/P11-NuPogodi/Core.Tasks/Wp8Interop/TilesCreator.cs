using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Microsoft.Phone.Shell;

namespace Win8.Core.Tasks.Wp8Interop
{
    public static class TilesCreator
    {
        #region From StandardTile

        public static ShellTileData CreateFromStandardTile(StandardTileData standardtiledata, Uri smallBackgroundImage)
        {
            return CreateFlipTile(standardtiledata.Title, standardtiledata.BackTitle, standardtiledata.BackContent, null,
                standardtiledata.Count, smallBackgroundImage, standardtiledata.BackgroundImage, standardtiledata.BackBackgroundImage, null, null);
        }

        public static ShellTileData CreateFromStandardTile(StandardTileData standardtiledata, Uri smallBackgroundImage,
            String wideBackContent, Uri wideBackgroundImage, Uri wideBackBackgroundImage)
        {
            return CreateFlipTile(standardtiledata.Title, standardtiledata.BackTitle, standardtiledata.BackContent,
                wideBackContent, standardtiledata.Count, smallBackgroundImage, standardtiledata.BackgroundImage,
                standardtiledata.BackBackgroundImage, wideBackgroundImage, wideBackBackgroundImage);
        }

        #endregion

        #region FlipTile

        public static ShellTileData CreateFlipTile(string title, string backTitle, string backContent, int? count,
            Uri smallBackgroundImage, Uri backgroundImage, Uri backBackgroundImage)
        {
            return CreateFlipTile(title, backTitle, backContent, null, count, smallBackgroundImage, backgroundImage,
                backBackgroundImage, null, null);
        }

        private static ShellTileData CreateFlipTile(string title, string backTitle, string backContent,
            string wideBackContent, int? count, Uri smallBackgroundImage, Uri backgroundImage, Uri backBackgroundImage,
            Uri wideBackgroundImage, Uri wideBackBackgroundImage)
        {
            object mynewtile = FlipTileData.FlipTileDataType.GetConstructor(new Type[] {}).Invoke(null);
            Utils.SetProperty(mynewtile, "Title", title);
            Utils.SetProperty(mynewtile, "Count", count);
            Utils.SetProperty(mynewtile, "BackTitle", backTitle);
            Utils.SetProperty(mynewtile, "BackContent", backContent);
            Utils.SetProperty(mynewtile, "SmallBackgroundImage", smallBackgroundImage);
            Utils.SetProperty(mynewtile, "BackgroundImage", backgroundImage);
            Utils.SetProperty(mynewtile, "BackBackgroundImage", backBackgroundImage);
            Utils.SetProperty(mynewtile, "WideBackgroundImage", wideBackgroundImage);
            Utils.SetProperty(mynewtile, "WideBackBackgroundImage", wideBackBackgroundImage);
            Utils.SetProperty(mynewtile, "WideBackContent", wideBackContent);
            return (ShellTileData) mynewtile;
        }

        #endregion

        #region IconicTile

        public static ShellTileData CreateIconicTile(string title, int? count, Color backgroundColor, Uri icon, Uri smallIcon)
        {
            return CreateIconicTile(title, count, backgroundColor, icon, smallIcon, null, null, null);
        }

        private static ShellTileData CreateIconicTile(string title, int? count, Color backgroundColor, Uri icon,
            Uri smallIcon, String wideTitle, String wideLine1, String wideLine2)
        {
            object mynewtile = IconicTileData.IconicTileDataType.GetConstructor(new Type[] {}).Invoke(null);
            Utils.SetProperty(mynewtile, "Title", title);
            Utils.SetProperty(mynewtile, "Count", count);
            Utils.SetProperty(mynewtile, "BackgroundColor", backgroundColor);
            Utils.SetProperty(mynewtile, "IconImage", icon);
            Utils.SetProperty(mynewtile, "SmallIconImage", smallIcon);
            Utils.SetProperty(mynewtile, "WideContent1", wideTitle);
            Utils.SetProperty(mynewtile, "WideContent2", wideLine1);
            Utils.SetProperty(mynewtile, "WideContent3", wideLine2);
            return (ShellTileData) mynewtile;
        }

        #endregion

        #region CyclicTile

        public static ShellTileData CreateCyclicTile(string title, int? count, Uri smallbackground, IEnumerable<Uri> images)
        {
            object mynewtile = CycleTileData.CycleTileDataType.GetConstructor(new Type[] {}).Invoke(null);
            Utils.SetProperty(mynewtile, "Title", title);
            Utils.SetProperty(mynewtile, "Count", count);
            Utils.SetProperty(mynewtile, "SmallBackgroundImage", smallbackground);
            Utils.SetProperty(mynewtile, "CycleImages", images.ToArray());
            return (ShellTileData) mynewtile;
        }

        #endregion
    }
}
