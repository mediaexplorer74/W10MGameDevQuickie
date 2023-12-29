// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.HoleTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;

#nullable disable
namespace GameManager.level.tile
{
  public class HoleTile : Tile
  {
    private int col;
    private int transitionColor1;
    private int transitionColor2;

    public HoleTile(int id)
      : base(id)
    {
      this.ConnectsToSand = true;
      this.ConnectsToWater = true;
      this.ConnectsToLava = true;
      this.col = Color.Get(111, 111, 110, 110);
      this.transitionColor1 = Color.Get(3, 111, Level.DirtColor - 111, Level.DirtColor);
      this.transitionColor2 = Color.Get(3, 111, Level.SandColor - 110, Level.SandColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      bool flag1 = !level.GetTile(x, y - 1).ConnectsToLiquid();
      bool flag2 = !level.GetTile(x, y + 1).ConnectsToLiquid();
      bool flag3 = !level.GetTile(x - 1, y).ConnectsToLiquid();
      bool flag4 = !level.GetTile(x + 1, y).ConnectsToLiquid();
      bool flag5 = flag1 && level.GetTile(x, y - 1).ConnectsToSand;
      bool flag6 = flag2 && level.GetTile(x, y + 1).ConnectsToSand;
      bool flag7 = flag3 && level.GetTile(x - 1, y).ConnectsToSand;
      bool flag8 = flag4 && level.GetTile(x + 1, y).ConnectsToSand;
      if (!flag1 && !flag3)
        screen.Render(x * 16, y * 16, 0, this.col, 0);
      else
        screen.Render(x * 16, y * 16, (flag3 ? 14 : 15) + (flag1 ? 0 : 1) * 32, flag5 || flag7 ? this.transitionColor2 : this.transitionColor1, 0);
      if (!flag1 && !flag4)
        screen.Render(x * 16 + 8, y * 16, 1, this.col, 0);
      else
        screen.Render(x * 16 + 8, y * 16, (flag4 ? 16 : 15) + (flag1 ? 0 : 1) * 32, flag5 || flag8 ? this.transitionColor2 : this.transitionColor1, 0);
      if (!flag2 && !flag3)
        screen.Render(x * 16, y * 16 + 8, 2, this.col, 0);
      else
        screen.Render(x * 16, y * 16 + 8, (flag3 ? 14 : 15) + (flag2 ? 2 : 1) * 32, flag6 || flag7 ? this.transitionColor2 : this.transitionColor1, 0);
      if (!flag2 && !flag4)
        screen.Render(x * 16 + 8, y * 16 + 8, 3, this.col, 0);
      else
        screen.Render(x * 16 + 8, y * 16 + 8, (flag4 ? 16 : 15) + (flag2 ? 2 : 1) * 32, flag6 || flag8 ? this.transitionColor2 : this.transitionColor1, 0);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => e.CanSwim();
  }
}
