using System;
using System.Reflection;
//using Microsoft.Phone.Shell;

namespace Win8.Core.Tasks.Wp8Interop
{
    // Summary:
    //     Describes a Tile template that flips from the front to the back side. Allows
    //     customization of the background image and text for both the front and back
    //     Tile.
    public class FlipTileData
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
        //     Gets or sets a background image of the back of the Tile. If this property
        //     is an empty URI, the background image of the back of the Tile will not change
        //     during an update.
        //
        // Returns:
        //     The background image of the back of the Tile.
        public Uri BackBackgroundImage
        {
            get { return (Uri) Utils.GetProperty(_shelltiledata, "BackBackgroundImage"); }
            set { Utils.SetProperty(_shelltiledata, "BackBackgroundImage", value); }
        }

        //
        // Summary:
        //     Gets or sets the text to display on the back of the Tile, above the title.
        //     If this property is an empty string, the content on the back of the Tile
        //     will not change during an update.
        //
        // Returns:
        //     The text to display on the back of the Tile, above the title.
        public string BackContent
        {
            get { return (string) Utils.GetProperty(_shelltiledata, "BackContent"); }
            set { Utils.SetProperty(_shelltiledata, "BackContent", value); }
        }

        //
        // Summary:
        //     Gets or sets the background image of the front of the Tile. If this property
        //     is an empty URI, the background image of the front of the Tile will not change
        //     during an update.
        //
        // Returns:
        //     The background image of the front of the Tile.
        public Uri BackgroundImage
        {
            get { return (Uri) Utils.GetProperty(_shelltiledata, "BackgroundImage"); }
            set { Utils.SetProperty(_shelltiledata, "BackgroundImage", value); }
        }

        //
        // Summary:
        //     Gets or sets the title to display at the bottom of the back of the Tile.
        //     If this property is an empty string, the title on the back of the Tile will
        //     not change during an update.
        //
        // Returns:
        //     The title to display at the bottom of the back of the Tile.
        public string BackTitle
        {
            get { return (string) Utils.GetProperty(_shelltiledata, "BackTitle"); }
            set { Utils.SetProperty(_shelltiledata, "BackTitle", value); }
        }

        //
        // Summary:
        //     Gets or sets a value between 1 and 99 will be displayed in the Count field
        //     of the Tile. A value of 0 means the Count will not be displayed. If this
        //     property is not set, the Count display will not change during an update.
        //
        // Returns:
        //     A value between 1 and 99 will be displayed in the Count field of the Tile.
        public int? Count
        {
            get { return (int?) Utils.GetProperty(_shelltiledata, "Count"); }
            set { Utils.SetProperty(_shelltiledata, "Count", value); }
        }

        // Summary:
        //     Gets and sets the front-side background image for the small Tile size.
        //
        // Returns:
        //     The front-side background image for the small Tile size.
        public Uri SmallBackgroundImage
        {
            get { return (Uri) Utils.GetProperty(_shelltiledata, "SmallBackgroundImage"); }
            set { Utils.SetProperty(_shelltiledata, "SmallBackgroundImage", value); }
        }

        //
        // Summary:
        //     Gets and sets the back-side background image for the wide Tile size.
        //
        // Returns:
        //     The back-side background image for the wide Tile size.
        public Uri WideBackBackgroundImage
        {
            get { return (Uri) Utils.GetProperty(_shelltiledata, "WideBackBackgroundImage"); }
            set { Utils.SetProperty(_shelltiledata, "WideBackBackgroundImage", value); }
        }

        //
        // Summary:
        //     Gets and sets the text that displays above the title, on the back-side of
        //     the wide Tile size.
        //
        // Returns:
        //     The text that displays above the title, on the back-side of the wide Tile
        //     size.
        public string WideBackContent
        {
            get { return (string) Utils.GetProperty(_shelltiledata, "WideBackContent"); }
            set { Utils.SetProperty(_shelltiledata, "WideBackContent", value); }
        }

        //
        // Summary:
        //     Gets and sets the front-side background image for the wide Tile size.
        //
        // Returns:
        //     The front-side background image for the wide Tile size.
        public Uri WideBackgroundImage
        {
            get { return (Uri) Utils.GetProperty(_shelltiledata, "WideBackgroundImage"); }
            set { Utils.SetProperty(_shelltiledata, "WideBackgroundImage", value); }
        }

        public static readonly Type FlipTileDataType = Utils.IsWP8
            ? Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone, Version=8.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e")
            : Type.GetType("Microsoft.Phone.Shell.FlipTileData, Microsoft.Phone");

        public FlipTileData()
        {
            _shelltiledata = FlipTileDataType.GetConstructor(new Type[] { }).Invoke(null);
        }

        private readonly object _shelltiledata;

        public ShellTileData ToShellTileData()
        {
            return (ShellTileData) _shelltiledata;
        }
    }
}
