// Decompiled with JetBrains decompiler
// Type: GameEngine.MusicSettings
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using System.IO;

#nullable disable
namespace GameEngine
{
  public class MusicSettings : ISerializeBinary
  {
    public float Volume;
    public bool MusicEnabled;
    public bool SoundEnabled;

    public string FileVersion => "1.0";

    public void Load(string version, BinaryReader reader)
    {
      this.MusicEnabled = reader.ReadBoolean();
      this.Volume = (float) reader.ReadDouble();
      this.SoundEnabled = reader.ReadBoolean();
    }

    public void Save(BinaryWriter writer)
    {
      writer.Write(this.MusicEnabled);
      writer.Write((double) this.Volume);
      writer.Write(this.SoundEnabled);
    }
  }
}
