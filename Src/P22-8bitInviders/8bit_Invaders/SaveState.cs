// Decompiled with JetBrains decompiler
// Type: GameManager.SaveState
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using GameEngine;

#nullable disable
namespace GameManager
{
  public class SaveState
  {
    public int Level;
    public int Score;
    public int Lives;
    public int lastOneUp;
    public bool isNewGame;

    public SaveState()
    {
      this.Level = 1;
      this.Score = 0;
      this.Lives = 3;
      this.lastOneUp = 0;
      this.isNewGame = true;
    }

    public void AdvanceLevel()
    {
      int level = this.Level;
      ++this.Level;
      Tools.Trace("ADVANCED CAMPAIGN FROM " + (object) level + " TO " + (object) this.Level);
    }
  }
}
