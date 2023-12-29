// Decompiled with JetBrains decompiler
// Type: GameManager.miniprefs
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using System.IO;
using System.IO.IsolatedStorage;

#nullable disable
namespace GameManager
{
  public static class miniprefs
  {
    private const int version = 1;
    private const string filename = "miniprefs";
    private const int gameWidthNorm = 200;
    private const int gameHeightNorm = 120;
    private static int screenSize = 0;
    private static bool useSound = true;
    private static bool autosave = true;
    private static bool miniMap = false;

    public static int ScreenSize
    {
      get => miniprefs.screenSize;
      set
      {
        if (value == miniprefs.screenSize)
          return;
        miniprefs.screenSize = value % 2;
      }
    }

    public static string ScreenSizeString
    {
      get
      {
        switch (miniprefs.screenSize)
        {
          case 0:
            return "normal";
          case 1:
            return "big";
          default:
            return "unknown";
        }
      }
    }

        public static int GameWidth
        {
            get
            {
                return miniprefs.screenSize == 1 ? 400 : 200;
            }
        }

        public static int GameHeight
        {
            get
            {
                return miniprefs.screenSize == 1 ? 240 : 120;
            }
        }

        public static bool UseSound
        {
            get
            {
                return miniprefs.useSound;
            }

            set
            {
                if (value == miniprefs.useSound)
                    return;
                miniprefs.useSound = value;
            }
        }

        public static string UseSoundString
        {
            get
            {
                return miniprefs.bool2String(miniprefs.useSound);
            }
        }

        public static bool Autosave
    {
      get => miniprefs.autosave;
      set
      {
        if (value == miniprefs.autosave)
          return;
        miniprefs.autosave = value;
      }
    }

        public static string AutosaveString
        {
            get
            {
                return miniprefs.bool2String(miniprefs.autosave);
            }
        }

        public static bool Minimap
    {
      get => miniprefs.miniMap;
      set
      {
        if (miniprefs.miniMap == value)
          return;
        miniprefs.miniMap = value;
      }
    }

    public static string MinimapString
    {
        get
        {
            return miniprefs.bool2String(miniprefs.miniMap);
        }
    }

    private static string bool2String(bool b)
    {
        return b ? "on" : "off";
    }

    public static void Load()
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          using (IsolatedStorageFileStream input = new IsolatedStorageFileStream(nameof (miniprefs), FileMode.Open, storeForApplication))
          {
            using (BinaryReader reader = new BinaryReader((Stream) input))
              miniprefs.loadFromReader(reader);
          }
        }
      }
      catch
      {
      }
    }

    public static void Save()
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          using (IsolatedStorageFileStream output = new IsolatedStorageFileStream(nameof (miniprefs), FileMode.Create, storeForApplication))
          {
            using (BinaryWriter writer = new BinaryWriter((Stream) output))
              miniprefs.saveToWriter(writer);
          }
        }
      }
      catch
      {
      }
    }

    private static void loadFromReader(BinaryReader reader)
    {
      if (reader.ReadInt32() > 1)
        return;
      miniprefs.screenSize = reader.ReadInt32();
      miniprefs.useSound = reader.ReadBoolean();
      miniprefs.autosave = reader.ReadBoolean();
      miniprefs.miniMap = reader.ReadBoolean();
    }

    private static void saveToWriter(BinaryWriter writer)
    {
      writer.Write(1);
      writer.Write(miniprefs.screenSize);
      writer.Write(miniprefs.useSound);
      writer.Write(miniprefs.autosave);
      writer.Write(miniprefs.miniMap);
    }
  }
}
