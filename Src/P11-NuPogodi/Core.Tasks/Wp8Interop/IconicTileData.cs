using System;
using System.Windows.Media;
using Microsoft.Phone.Shell;

namespace Win8.Core.Tasks.Wp8Interop
{
    public class IconicTileData
    {
        // Summary:
        //     Gets or sets the text that displays on the front side of the medium and wide
        //     tile sizes.
        //
        // Returns:
        //     The text that displays on the front side of the medium and wide tile sizes.
        public string Title
        {
            get { return (string) Utils.GetProperty(_shelltiledata, "Title"); }
            set { Utils.SetProperty(_shelltiledata, "Title", value); }
        }

        // Summary:
        //     Gets or sets the background color of the Tile. Setting this property overrides
        //     the default theme color that is set on the phone.
        //
        // Returns:
        //     The background color of the Tile.
        public Color BackgroundColor
        {
            get { return (Color) Utils.GetProperty(_shelltiledata, "BackgroundColor"); }
            set { Utils.SetProperty(_shelltiledata, "BackgroundColor", value); }
        }

        //
        // Summary:
        //     Gets or sets a value between 1 and 99 that will be displayed in the Count
        //     field of the Tile. A value of 0 means the Count will not be displayed. If
        //     this property is not set, the Count display will not change during an update.
        //
        // Returns:
        //     A value between 1 and 99 that will be displayed in the Count field of the
        //     Tile.
        public int? Count
        {
            get { return (int?) Utils.GetProperty(_shelltiledata, "Count"); }
            set { Utils.SetProperty(_shelltiledata, "Count", value); }
        }
        //
        // Summary:
        //     Gets or sets the icon image for the medium and large Tile sizes.
        //
        // Returns:
        //     The icon image for the medium and large Tile sizes.
        public Uri IconImage
        {
            get { return (Uri) Utils.GetProperty(_shelltiledata, "IconImage"); }
            set { Utils.SetProperty(_shelltiledata, "IconImage", value); }
        }

        //
        // Summary:
        //     Gets or sets the icon image for the small Tile size.
        //
        // Returns:
        //     The icon image for the small Tile size.
        public Uri SmallIconImage
        {
            get { return (Uri) Utils.GetProperty(_shelltiledata, "SmallIconImage"); }
            set { Utils.SetProperty(_shelltiledata, "SmallIconImage", value); }
        }

        //
        // Summary:
        //     Gets or sets the text that displays on the first row of the wide Tile size.
        //
        // Returns:
        //     The text that displays on the first row of the wide Tile size.
        public string WideContent1
        {
            get { return (string) Utils.GetProperty(_shelltiledata, "WideContent1"); }
            set { Utils.SetProperty(_shelltiledata, "WideContent1", value); }
        }

        //
        // Summary:
        //     Gets or sets the text that displays on the second row of the wide Tile size.
        //
        // Returns:
        //     The text that displays on the second row of the wide Tile size.
        public string WideContent2
        {
            get { return (string) Utils.GetProperty(_shelltiledata, "WideContent2"); }
            set { Utils.SetProperty(_shelltiledata, "WideContent2", value); }
        }

        //
        // Summary:
        //     Gets or sets the text that displays on the third row of the wide Tile size.
        //
        // Returns:
        //     The text that displays on the third row of the wide Tile size.
        public string WideContent3
        {
            get { return (string) Utils.GetProperty(_shelltiledata, "WideContent3"); }
            set { Utils.SetProperty(_shelltiledata, "WideContent3", value); }
        }

        public static readonly Type IconicTileDataType = Utils.IsWP8
            ? Type.GetType("Microsoft.Phone.Shell.IconicTileData, Microsoft.Phone, Version=8.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e")
            : Type.GetType("Microsoft.Phone.Shell.IconicTileData, Microsoft.Phone");

        public IconicTileData()
        {
            _shelltiledata = IconicTileDataType.GetConstructor(new Type[] { }).Invoke(null);
        }

        private readonly object _shelltiledata;

        public ShellTileData ToShellTileData()
        {
            return (ShellTileData) _shelltiledata;
        }
    }
}
