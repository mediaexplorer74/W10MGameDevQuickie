// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.FlowerTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using GameManager.item;
using GameManager.item.resource;

#nullable disable
namespace GameManager.level.tile
{
  public class FlowerTile : GrassTile
  {
    private int flowerCol;

    public FlowerTile(int id)
      : base(id)
    {
      Tile.Tiles[id] = (Tile) this;
      this.ConnectsToGrass = true;
      this.flowerCol = Color.Get(10, Level.GrassColor, 555, 440);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      base.Render(screen, level, x, y);
      int num = level.GetData(x, y) / 16 % 2;
      if (num == 0)
        screen.Render(x * 16, y * 16, 33, this.flowerCol, 0);
      if (num == 1)
        screen.Render(x * 16 + 8, y * 16, 33, this.flowerCol, 0);
      if (num == 1)
        screen.Render(x * 16, y * 16 + 8, 33, this.flowerCol, 0);
      if (num != 0)
        return;
      screen.Render(x * 16 + 8, y * 16 + 8, 33, this.flowerCol, 0);
    }

    public override bool Interact(
      Level level,
      int x,
      int y,
      Player player,
      Item item,
      int attackDir)
    {
      if (item is ToolItem)
      {
        ToolItem toolItem = (ToolItem) item;
        if (toolItem.MyType == ToolType.Shovel && player.PayStamina(4 - toolItem.ToolLevel))
        {
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Flower), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Flower), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
          level.SetTile(x, y, Tile.Grass, 0);
          return true;
        }
      }
      return false;
    }

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
      int num = this.random.Next(2) + 1;
      for (int index = 0; index < num; ++index)
        level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Flower), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
      level.SetTile(x, y, Tile.Grass, 0);
    }
  }
}
