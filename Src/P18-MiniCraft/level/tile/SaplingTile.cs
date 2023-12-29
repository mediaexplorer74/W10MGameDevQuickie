// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.SaplingTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;

#nullable disable
namespace GameManager.level.tile
{
  public class SaplingTile : Tile
  {
    private Tile onType;
    private Tile growsTo;
    private int col;

    public SaplingTile(int id, Tile onType, Tile growsTo)
      : base(id)
    {
      this.onType = onType;
      this.growsTo = growsTo;
      this.ConnectsToSand = onType.ConnectsToSand;
      this.ConnectsToGrass = onType.ConnectsToGrass;
      this.ConnectsToWater = onType.ConnectsToWater;
      this.ConnectsToLava = onType.ConnectsToLava;
      this.col = Color.Get(10, 40, 50, -1);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      this.onType.Render(screen, level, x, y);
      screen.Render(x * 16 + 4, y * 16 + 4, 107, this.col, 0);
    }

    public override void Tick(Level level, int x, int y)
    {
      int val = level.GetData(x, y) + 1;
      if (val > 100)
        level.SetTile(x, y, this.growsTo, 0);
      else
        level.SetData(x, y, val);
    }

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
      level.SetTile(x, y, this.onType, 0);
    }
  }
}
