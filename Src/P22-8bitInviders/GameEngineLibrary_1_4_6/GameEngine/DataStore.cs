// Decompiled with JetBrains decompiler
// Type: GameEngine.DataStore
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;

#nullable disable
namespace GameEngine
{
  public static class DataStore
  {
    public static T Load<T>(string path)
    {
      T obj = default (T);
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(path))
          {
            using (Stream input = (Stream) storeForApplication.OpenFile(path, FileMode.Open))
            {
              BinaryReader reader = new BinaryReader(input);
              string version = reader.ReadString();
              T instance = Activator.CreateInstance<T>();
              ((ISerializeBinary) (object) instance).Load(version, reader);
              obj = instance;
            }
          }
        }
      }
      catch
      {
        obj = default (T);
      }
      return obj;
    }

    public static void Save<T>(string path, T item)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          string directoryName = Path.GetDirectoryName(path);
          if (!string.IsNullOrEmpty(directoryName) && !storeForApplication.DirectoryExists(directoryName))
            storeForApplication.CreateDirectory(directoryName);
          using (Stream output = (Stream) storeForApplication.OpenFile(path, FileMode.Create))
          {
            ISerializeBinary serializeBinary = (ISerializeBinary) (object) item;
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(serializeBinary.FileVersion);
            serializeBinary.Save(writer);
            writer.Flush();
          }
        }
      }
      catch
      {
      }
    }

    public static void DeleteDir(string path)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (!storeForApplication.DirectoryExists(path))
            return;
          storeForApplication.DeleteDirectory(path);
        }
      }
      catch
      {
      }
    }

    public static bool DeleteFile(string path)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(path))
          {
            storeForApplication.DeleteFile(path);
            return true;
          }
        }
      }
      catch
      {
      }
      return false;
    }

    public static bool FileExists(string path)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
          return storeForApplication.FileExists(path);
      }
      catch
      {
        return false;
      }
    }

    public static void Serialize<T>(string filename, T saveObject)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(filename))
          {
            using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(filename, FileMode.Create, storeForApplication))
              new XmlSerializer(typeof (T)).Serialize((Stream) storageFileStream, (object) saveObject);
          }
          else
          {
            using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(filename, FileMode.CreateNew, storeForApplication))
              new XmlSerializer(typeof (T)).Serialize((Stream) storageFileStream, (object) saveObject);
          }
        }
      }
      catch (Exception ex)
      {
      }
    }

    public static bool Deserialize<T>(string filename, out T items)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(filename))
          {
            using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(filename, FileMode.Open, storeForApplication))
            {
              XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
              items = (T) xmlSerializer.Deserialize((Stream) storageFileStream);
              return true;
            }
          }
          else
          {
            items = default (T);
            return false;
          }
        }
      }
      catch
      {
        items = default (T);
        return false;
      }
    }

    public static bool ExternalDeserialize<T>(string filename, out T items)
    {
      try
      {
        using (Stream stream = TitleContainer.OpenStream(filename))
        {
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
          items = (T) xmlSerializer.Deserialize(stream);
          return true;
        }
      }
      catch
      {
      }
      items = default (T);
      return false;
    }

    public static void SimpleSave<T>(string filename, T item)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(filename, FileMode.Create, storeForApplication))
          {
            using (StreamWriter streamWriter = new StreamWriter((Stream) storageFileStream))
              streamWriter.WriteLine((object) item);
          }
        }
      }
      catch
      {
      }
    }

    public static bool SimpleLoad_string(string filename, out string item)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(filename))
          {
            using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(filename, FileMode.Open, storeForApplication))
            {
              using (StreamReader streamReader = new StreamReader((Stream) storageFileStream))
                item = streamReader.ReadToEnd();
            }
            return true;
          }
          item = string.Empty;
          return false;
        }
      }
      catch
      {
        item = string.Empty;
        return false;
      }
    }

    public static bool SimpleLoad_Int32(string filename, out int item)
    {
      try
      {
        using (IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication())
        {
          if (storeForApplication.FileExists(filename))
          {
            using (IsolatedStorageFileStream storageFileStream = new IsolatedStorageFileStream(filename, FileMode.Open, storeForApplication))
            {
              using (StreamReader streamReader = new StreamReader((Stream) storageFileStream))
                item = int.Parse(streamReader.ReadLine(), (IFormatProvider) CultureInfo.InvariantCulture);
            }
            return true;
          }
          item = 0;
          return false;
        }
      }
      catch
      {
        item = 0;
        return false;
      }
    }
  }
}
