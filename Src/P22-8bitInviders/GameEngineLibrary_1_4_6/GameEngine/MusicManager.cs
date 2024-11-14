// Decompiled with JetBrains decompiler
// Type: GameEngine.MusicManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

#nullable disable
namespace GameEngine
{
  public static class MusicManager
  {
    private static MusicSettings SettingsObject;
    public static string currentlyPlaying = string.Empty;
    public static bool shouldConfirmMusic = true;
    public static bool IsMusicAllowed = true;
    public static float GlobalVolume = 0.5f;
    public static float DefaultGlobalVolume = 0.5f;
    public static bool AppEnableMusic = true;
    public static bool SoundEnabled = true;

    public static bool IsMusicPlaying
    {
        get
        {
            return MediaPlayer.State == MediaState.Playing;
        }
    }

    public static bool MusicCheck()
    {
      MusicManager.IsMusicAllowed = MediaPlayer.GameHasControl;
      if (!MusicManager.IsMusicAllowed)
        MusicManager.AppEnableMusic = false;
      return MusicManager.IsMusicAllowed;
    }

    public static void AllowEnableMusic()
    {
      MusicManager.IsMusicAllowed = MusicManager.AppEnableMusic = true;
    }

    public static void DisallowDisableMusic()
    {
      MusicManager.IsMusicAllowed = MusicManager.AppEnableMusic = false;
    }

    public static void Play(string name)
    {
        MusicManager.Play(name, false);
    }

    public static void Play(string name, bool loop)
    {
      if (!MusicManager.IsMusicAllowed)
        return;
      if (!MusicManager.AppEnableMusic)
        return;
      try
      {
        Uri _uri = new Uri(name + ".mp3"/*".wma"*/, UriKind.Relative);
        Song _song = Song.FromUri("song"/*"Bullet Defender - In Game"*/, _uri);
        MediaPlayer.Play(_song);
        MediaPlayer.IsRepeating = loop;
        MediaPlayer.Volume = MusicManager.GlobalVolume;
        MusicManager.currentlyPlaying = name;
      }
      catch (Exception ex)
      {
         Debug.WriteLine("[ex] MediaPlayer.Play error: " + ex.Message);
      }
    }

    public static void SetGlobalVolume(float value)
    { 
        MusicManager.GlobalVolume = value; 
    }

    public static void SetCurrentVolume(float value)
    {
      if (!MusicManager.IsMusicAllowed)
        return;
      MediaPlayer.Volume = value;
    }

    public static void Stop()
    {
      if (!MusicManager.IsMusicAllowed)
        return;
      try
      {
        MediaPlayer.Stop();
      }
      catch
      {
      }
    }

    public static void SaveMusicSettings()
    {
      MusicManager.SettingsObject = new MusicSettings()
      {
        Volume = MusicManager.GlobalVolume,
        MusicEnabled = MusicManager.AppEnableMusic,
        SoundEnabled = MusicManager.SoundEnabled
      };
      DataStore.Save<MusicSettings>("musicsettings.bin", MusicManager.SettingsObject);
    }

    public static void LoadMusicSettings()
    { 
        MusicManager.LoadMusicSettings(0.5f); 
    }

    public static void LoadMusicSettings(float defaultVolume)
    {
      MusicManager.DefaultGlobalVolume = defaultVolume;
      MusicManager.SettingsObject = DataStore.Load<MusicSettings>("musicsettings.bin");
      if (MusicManager.SettingsObject == null)
      {
        MusicManager.GlobalVolume = MusicManager.DefaultGlobalVolume;
        MusicManager.AppEnableMusic = true;
        MusicManager.SoundEnabled = true;
      }
      else
      {
        MusicManager.GlobalVolume = MusicManager.SettingsObject.Volume;
        MusicManager.AppEnableMusic = MusicManager.SettingsObject.MusicEnabled;
        MusicManager.SoundEnabled = MusicManager.SettingsObject.SoundEnabled;
      }
      MusicManager.MusicCheck();
    }
  }
}
