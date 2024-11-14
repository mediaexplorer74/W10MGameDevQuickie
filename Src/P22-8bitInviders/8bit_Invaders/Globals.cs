// Decompiled with JetBrains decompiler
// Type: GameManager.Globals
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;

#nullable disable
namespace GameManager
{
  public static class Globals
  {
    public static SaveGameManager saveManager => ((Game1) Engine.game).saveManager;

    public static OptionsState optionsState => ((Game1) Engine.game).saveManager.optionsState;
  }
}
