// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.CactusTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.entity.particle;
using GameManager.gfx;
using GameManager.item;
using GameManager.item.resource;

#nullable disable
namespace GameManager.level.tile
{
  public class CactusTile : Tile
  {
    private int col;

    public CactusTile(int id)
      : base(id)
    {
      this.ConnectsToSand = true;
      this.col = Color.Get(20, 40, 50, Level.SandColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      screen.Render(x * 16, y * 16, 72, this.col, 0);
      screen.Render(x * 16 + 8, y * 16, 73, this.col, 0);
      screen.Render(x * 16, y * 16 + 8, 104, this.col, 0);
      screen.Render(x * 16 + 8, y * 16 + 8, 105, this.col, 0);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => false;

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
      int val = level.GetData(x, y) + dmg;
      level.Add((Entity) new SmashParticle(x * 16 + 8, y * 16 + 8));
      level.Add((Entity) new TextParticle(string.Empty + (object) dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));
      if (val >= 10)
      {
        int num = this.random.Next(2) + 1;
        for (int index = 0; index < num; ++index)
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.CactusFlower), x * 16 + (this.random.Next(10) + 3), y * 16 + (this.random.Next(10) + 3)));
        level.SetTile(x, y, Tile.Sand, 0);
      }
      else
        level.SetData(x, y, val);
    }

    public override void BumpedInto(Level level, int x, int y, Entity entity)
    {
      entity.Hurt((Tile) this, x, y, 1);
    }

    public override void Tick(Level level, int xt, int yt)
    {
      int data = level.GetData(xt, yt);
      if (data <= 0)
        return;
      level.SetData(xt, yt, data - 1);
    }
  }
}
