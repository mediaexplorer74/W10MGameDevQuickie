// Decompiled with JetBrains decompiler
// Type: GameEngine.AudioManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameEngine
{
  public class AudioManager
  {
    private GameScreen Screen;
    private Dictionary<string, SoundEffectInstance> audioLoopingClips = new Dictionary<string, SoundEffectInstance>();
    private Dictionary<string, SoundEffect> audioClips = new Dictionary<string, SoundEffect>();
    public float timeBetweenPlays;
    private float _timeBetweenPlays;
    public int soundsAtOnce = 4;
    private int soundsPlayed;

    public AudioManager(GameScreen screen) => this.Screen = screen;

    public void AddSound(string name, string path)
    {
      try
      {
        SoundEffect soundEffect = this.Screen.content.Load<SoundEffect>(path);
        if (this.audioClips.ContainsKey(name))
          this.audioClips.Remove(name);
        this.audioClips.Add(name, soundEffect);
      }
      catch
      {
        Tools.Trace("Sound not found: " + name);
      }
    }

    public void AddLoopingSound(string name, string path, bool isLoop)
    {
      try
      {
        using (Stream stream = TitleContainer.OpenStream(path + ".wav"))
        {
          SoundEffectInstance instance = SoundEffect.FromStream(stream).CreateInstance();
          if (instance == null)
            return;
          instance.IsLooped = isLoop;
          if (this.audioLoopingClips.ContainsKey(name))
            this.audioLoopingClips.Remove(name);
          this.audioLoopingClips.Add(name, instance);
        }
      }
      catch
      {
        Tools.Trace("AddLoopingSound not found: " + name);
      }
    }

    public void AddLoopingSound(string name, string path)
    {
      try
      {
        this.AddLoopingSound(name, path, true);
      }
      catch
      {
        Tools.Trace("AddLoopingSound not found: " + name);
      }
    }

    public void Play(string name)
    {
      if (this.Screen.IsExiting || !MusicManager.SoundEnabled || this.soundsPlayed > this.soundsAtOnce)
        return;
      if (this.audioClips.ContainsKey(name))
      {
        try
        {
          ++this.soundsPlayed;
          this.audioClips[name].Play();
        }
        catch
        {
        }
      }
      else
      {
        if (!this.audioLoopingClips.ContainsKey(name))
          return;
        SoundEffectInstance audioLoopingClip = this.audioLoopingClips[name];
        if (audioLoopingClip.State == SoundState.Playing)
          audioLoopingClip.Stop(true);
        try
        {
          audioLoopingClip.Play();
        }
        catch
        {
        }
      }
    }

    public void PlayLoopIfNotPlaying(string name)
    {
      if (this.Screen.IsExiting || !MusicManager.SoundEnabled || this.soundsPlayed > this.soundsAtOnce || !this.audioLoopingClips.ContainsKey(name))
        return;
      SoundEffectInstance audioLoopingClip = this.audioLoopingClips[name];
      if (audioLoopingClip.State == SoundState.Playing)
        return;
      try
      {
        audioLoopingClip.Play();
      }
      catch
      {
      }
    }

    public void Stop(string name)
    {
      if (this.Screen.IsExiting || !this.audioLoopingClips.ContainsKey(name))
        return;
      this.audioLoopingClips[name].Stop();
    }

    public SoundState LoopingClipStatus(string name)
    {
      return this.audioLoopingClips.ContainsKey(name) ? this.audioLoopingClips[name].State : SoundState.Stopped;
    }

    public void Update(GameTime gameTime)
    {
      this._timeBetweenPlays += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this._timeBetweenPlays < (double) this.timeBetweenPlays)
        return;
      this.soundsPlayed = 0;
    }
  }
}
