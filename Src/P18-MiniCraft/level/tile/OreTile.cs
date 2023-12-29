// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.OreTile
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
  public class OreTile : Tile
  {
    private Resource toDrop;
    private int color;

    public OreTile(int id, Resource toDrop)
      : base(id)
    {
      this.toDrop = toDrop;
      this.color = toDrop.ResColor & 16776960;
      this.color = (int) ((long) toDrop.ResColor & 4294967040L) + Color.Get(Level.DirtColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      screen.Render(x * 16, y * 16, 49, this.color, 0);
      screen.Render(x * 16 + 8, y * 16, 50, this.color, 0);
      screen.Render(x * 16, y * 16 + 8, 81, this.color, 0);
      screen.Render(x * 16 + 8, y * 16 + 8, 82, this.color, 0);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => false;

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
      int num = this.random.Next(2);
      if (val >= this.random.Next(10) + 3)
      {
        level.SetTile(x, y, Tile.Dirt, 0);
        num += 2;
      }
      else
        level.SetData(x, y, val);
      for (int index = 0; index < num; ++index)
        level.Add((Entity) new ItemEntity((Item) new ResourceItem(this.toDrop), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
    }

    public override void BumpedInto(Level level, int x, int y, Entity entity)
    {
      entity.Hurt((Tile) this, x, y, 3);
    }
  }
}
