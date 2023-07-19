using System;
using System.Reflection;

namespace Win8.Core.Tasks.Wp8Interop
{
    public static class ShellTileExt
    {
        public static readonly Type ShellTileType = Utils.IsWP8
            ? Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone, Version=8.0.0.0, Culture=neutral, PublicKeyToken=24eec0d8c86cda1e")
            : Type.GetType("Microsoft.Phone.Shell.ShellTile, Microsoft.Phone");

        public static void Create(Uri uri, ShellTileData tiledata, bool usewide)
        {
            MethodInfo createmethod = ShellTileType.GetMethod("Create", new[] { 
                typeof(Uri), typeof(ShellTileData), typeof(bool) });
            createmethod.Invoke(null, new object[] {uri, tiledata, usewide});
        }

        public static void Update(ShellTile tile, ShellTileData tiledata)
        {
            ShellTileType.GetMethod("Update").Invoke(tile, new Object[] { tiledata });
        }
    }
}
