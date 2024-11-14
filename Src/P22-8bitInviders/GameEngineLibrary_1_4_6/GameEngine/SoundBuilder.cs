// Decompiled with JetBrains decompiler
// Type: GameEngine.SoundBuilder
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
  public class SoundBuilder
  {
    private Dictionary<string, SoundClip> voiceClips = new Dictionary<string, SoundClip>();

    public void AddClip(string name, string path)
    {
      using (Stream stream = TitleContainer.OpenStream(path + ".wav"))
      {
        SoundEffect soundEffect = SoundEffect.FromStream(stream);
        SoundClip soundClip = new SoundClip(soundEffect.Duration.TotalSeconds, soundEffect.CreateInstance());
        this.voiceClips.Add(name, soundClip);
      }
    }

    public void BuildAndPlaySentence(params string[] clipNames)
    {
      Timer[] timerArray = new Timer[clipNames.Length - 1];
      float num = 0.0f;
      for (int index = 0; index < timerArray.Length; ++index)
      {
        int next = index + 1;
        timerArray[index] = new Timer("timer" + index.ToString());
        num += (float) this.voiceClips[clipNames[index]].Duration;
        timerArray[index].SecondsUntilExpire = num;
        timerArray[index].ExpiredEvent += (TimerExpiredHandler) (U => this.voiceClips[clipNames[next]].Play());
        timerArray[index].Start();
      }
      this.voiceClips[clipNames[0]].Play();
    }
  }
}
