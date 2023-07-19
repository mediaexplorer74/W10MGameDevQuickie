using System;
using System.Collections.Generic;
using System.Reflection;
//using Microsoft.Phone.Shell;

namespace Win8.Core.Tasks.Wp8Interop
{
    public class CycleTileData
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
        //     Gets or sets a collection of up to 9 background images for the medium and
        //     wide Tile sizes.
        //
        // Returns:
        //     A collection of up to 9 background images for the medium and wide Tile sizes.
        public IEnumerable<Uri> CycleImages
        {
            get { return (IEnumerable<Uri>) Utils.GetProperty(_shelltiledata, "CycleImages"); }
            set { Utils.SetProperty(_shelltiledata, "CycleImages", value); }
        }

        //
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

        public static readonly Type CycleTileDataType = Utils.IsWP8
            ? Type.GetType(
                "Microsoft.Phone.Shell.CycleTileData, Microsoft.Phone, Version=8.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e")
            : Type.GetType(
                "Microsoft.Phone.Shell.CycleTileData, Microsoft.Phone");

        public CycleTileData()
        {
            _shelltiledata = CycleTileDataType.GetConstructor(new Type[] { }).Invoke(null);
        }

        private readonly object _shelltiledata;

        public ShellTileData ToShellTileData()
        {
            return (ShellTileData) _shelltiledata;
        }
    }
}
