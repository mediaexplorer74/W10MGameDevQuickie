// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.WaterTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using System;

#nullable disable
namespace GameManager.level.tile
{
  public class WaterTile : Tile
  {
    private int col;
    private int transitionColor1;
    private int transitionColor2;
    private Random wRandom;

    public WaterTile(int id)
      : base(id)
    {
      this.ConnectsToSand = true;
      this.ConnectsToWater = true;
      this.col = Color.Get(5, 5, 115, 115);
      this.transitionColor1 = Color.Get(3, 5, Level.DirtColor - 111, Level.DirtColor);
      this.transitionColor2 = Color.Get(3, 5, Level.SandColor - 110, Level.SandColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      this.wRandom = new Random((Tile.TickCount + (x / 2 - y) * 4311) / 10 * 54687121 + x * 3271612 + y * 341298716);
      bool flag1 = !level.GetTile(x, y - 1).ConnectsToWater;
      bool flag2 = !level.GetTile(x, y + 1).ConnectsToWater;
      bool flag3 = !level.GetTile(x - 1, y).ConnectsToWater;
      bool flag4 = !level.GetTile(x + 1, y).ConnectsToWater;
      bool flag5 = flag1 && level.GetTile(x, y - 1).ConnectsToSand;
      bool flag6 = flag2 && level.GetTile(x, y + 1).ConnectsToSand;
      bool flag7 = flag3 && level.GetTile(x - 1, y).ConnectsToSand;
      bool flag8 = flag4 && level.GetTile(x + 1, y).ConnectsToSand;
      if (!flag1 && !flag3)
        screen.Render(x * 16, y * 16, this.wRandom.Next(4), this.col, this.wRandom.Next(4));
      else
        screen.Render(x * 16, y * 16, (flag3 ? 14 : 15) + (flag1 ? 0 : 1) * 32, flag5 || flag7 ? this.transitionColor2 : this.transitionColor1, 0);
      if (!flag1 && !flag4)
        screen.Render(x * 16 + 8, y * 16, this.wRandom.Next(4), this.col, this.wRandom.Next(4));
      else
        screen.Render(x * 16 + 8, y * 16, (flag4 ? 16 : 15) + (flag1 ? 0 : 1) * 32, flag5 || flag8 ? this.transitionColor2 : this.transitionColor1, 0);
      if (!flag2 && !flag3)
        screen.Render(x * 16, y * 16 + 8, this.wRandom.Next(4), this.col, this.wRandom.Next(4));
      else
        screen.Render(x * 16, y * 16 + 8, (flag3 ? 14 : 15) + (flag2 ? 2 : 1) * 32, flag6 || flag7 ? this.transitionColor2 : this.transitionColor1, 0);
      if (!flag2 && !flag4)
        screen.Render(x * 16 + 8, y * 16 + 8, this.wRandom.Next(4), this.col, this.wRandom.Next(4));
      else
        screen.Render(x * 16 + 8, y * 16 + 8, (flag4 ? 16 : 15) + (flag2 ? 2 : 1) * 32, flag6 || flag8 ? this.transitionColor2 : this.transitionColor1, 0);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => e.CanSwim();

    public override void Tick(Level level, int xt, int yt)
    {
      int x = xt;
      int y = yt;
      if (this.random.Next(2) == 0)
        x += this.random.Next(2) * 2 - 1;
      else
        y += this.random.Next(2) * 2 - 1;
      if (level.GetTile(x, y) != Tile.Hole)
        return;
      level.SetTile(x, y, (Tile) this, 0);
    }
  }
}
