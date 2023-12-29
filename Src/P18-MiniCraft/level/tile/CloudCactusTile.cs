// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.CloudCactusTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.entity.particle;
using GameManager.gfx;
using GameManager.item;

#nullable disable
namespace GameManager.level.tile
{
  public class CloudCactusTile : Tile
  {
    private int color;

    public CloudCactusTile(int id)
      : base(id)
    {
      this.color = Color.Get(444, 111, 333, 555);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      screen.Render(x * 16, y * 16, 49, this.color, 0);
      screen.Render(x * 16 + 8, y * 16, 50, this.color, 0);
      screen.Render(x * 16, y * 16 + 8, 81, this.color, 0);
      screen.Render(x * 16 + 8, y * 16 + 8, 82, this.color, 0);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => e is AirWizard;

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
      this.Hurt(level, x, y, 0);
    }

    public override bool Interact(
      Level level,
      int xt,
      int yt,
      Player player,
      Item item,
      int attackDir)
    {
      if (item is ToolItem)
      {
        ToolItem toolItem = (ToolItem) item;
        if (toolItem.MyType == ToolType.Pickaxe && player.PayStamina(6 - toolItem.ToolLevel))
        {
          this.Hurt(level, xt, yt, 1);
          return true;
        }
      }
      return false;
    }

    public void Hurt(Level level, int x, int y, int dmg)
    {
      int val = level.GetData(x, y) + 1;
      level.Add((Entity) new SmashParticle(x * 16 + 8, y * 16 + 8));
      level.Add((Entity) new TextParticle(string.Empty + (object) dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));
      if (dmg <= 0)
        return;
      if (val >= 10)
        level.SetTile(x, y, Tile.Cloud, 0);
      else
        level.SetData(x, y, val);
    }

    public override void BumpedInto(Level level, int x, int y, Entity entity)
    {
      if (entity is AirWizard)
        return;
      entity.Hurt((Tile) this, x, y, 3);
    }
  }
}
