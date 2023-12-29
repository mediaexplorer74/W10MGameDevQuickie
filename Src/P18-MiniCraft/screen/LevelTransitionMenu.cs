// Decompiled with JetBrains decompiler
// Type: GameManager.screen.LevelTransitionMenu


using GameManager.gfx;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class LevelTransitionMenu : Menu
  {
    private int dir;
    private int time;

        public LevelTransitionMenu(int dir)
        {
            this.dir = dir;
        }

        public LevelTransitionMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
      this.time += 2;
      if (this.time == 30)
        this.game.ChangeLevel(this.dir);
      if (this.time != 60)
        return;
      this.game.SetMenu((Menu) null);
    }

    public override void Render(Screen screen)
    {
      for (int index1 = 0; index1 < 20; ++index1)
      {
        for (int index2 = 0; index2 < 15; ++index2)
        {
          int num = index2 + index1 % 2 * 2 + index1 / 3 - this.time;
          if (num < 0 && num > -30)
          {
            if (this.dir > 0)
              screen.Render(index1 * 8, index2 * 8, 0, 0, 0);
            else
              screen.Render(index1 * 8, screen.H - index2 * 8 - 8, 0, 0, 0);
          }
        }
      }
    }

    public override void SaveToWriter(BinaryWriter writer)
    {
      base.SaveToWriter(writer);
      writer.Write(this.dir);
      writer.Write(this.time);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.dir = reader.ReadInt32();
      this.time = reader.ReadInt32();
    }
  }
}
