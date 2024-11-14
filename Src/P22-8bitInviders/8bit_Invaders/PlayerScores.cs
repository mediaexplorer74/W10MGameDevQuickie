// Decompiled with JetBrains decompiler
// Type: GameManager.PlayerScores
// Assembly: 8bit_Invaders, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 255A7773-2619-4586-8792-58D6F22E861B
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\8bit_Invaders.dll

using System;

#nullable disable
namespace GameManager
{
  public class PlayerScores
  {
    public int[] Scores;

    public PlayerScores()
    {
      this.Scores = new int[10];
      for (int index = 0; index < 10; ++index)
        this.Scores[index] = 0;
    }

    public void AddHiScore(int amount)
    {
      int num = amount;
      int index1 = -1;
      for (int index2 = 0; index2 < 10; ++index2)
      {
        if (this.Scores[index2] < num)
        {
          num = this.Scores[index2];
          index1 = index2;
        }
      }
      if (index1 <= -1)
        return;
      this.Scores[index1] = amount;
      Array.Sort<int>(this.Scores);
      Globals.saveManager.SaveOptionsHiscores();
    }

    public void Reset()
    {
      for (int index = 0; index < 10; ++index)
        this.Scores[index] = 0;
      Globals.saveManager.SaveOptionsHiscores();
    }
  }
}
