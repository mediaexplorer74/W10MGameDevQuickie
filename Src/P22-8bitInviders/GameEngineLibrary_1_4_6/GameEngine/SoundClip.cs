// Decompiled with JetBrains decompiler
// Type: GameEngine.SoundClip
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework.Audio;

#nullable disable
namespace GameEngine
{
  public class SoundClip
  {
    public double Duration;
    private SoundEffectInstance sfxInstance;

    public SoundClip(double d, SoundEffectInstance sfi)
    {
      this.Duration = d;
      this.sfxInstance = sfi;
    }

    public void Play() => this.sfxInstance.Play();
  }
}
