// Decompiled with JetBrains decompiler
// Type: GameManager.OptionsState
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

#nullable disable
namespace GameManager
{
  public class OptionsState
  {
    public bool usingTouch;
    public bool autoShoot;

    public OptionsState()
    {
      this.usingTouch = true;
      this.autoShoot = true;
    }
  }
}
