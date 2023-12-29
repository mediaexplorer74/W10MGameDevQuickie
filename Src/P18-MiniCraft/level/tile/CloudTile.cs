// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.CloudTile
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
  public class CloudTile : Tile
  {
    private int col;
    private int transitionColor;

    public CloudTile(int id)
      : base(id)
    {
      this.col = Color.Get(444, 444, 555, 555);
      this.transitionColor = Color.Get(333, 444, 555, -1);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      bool flag1 = level.GetTile(x, y - 1) == Tile.InfiniteFall;
      bool flag2 = level.GetTile(x, y + 1) == Tile.InfiniteFall;
      bool flag3 = level.GetTile(x - 1, y) == Tile.InfiniteFall;
      bool flag4 = level.GetTile(x + 1, y) == Tile.InfiniteFall;
      bool flag5 = level.GetTile(x - 1, y - 1) == Tile.InfiniteFall;
      bool flag6 = level.GetTile(x - 1, y + 1) == Tile.InfiniteFall;
      bool flag7 = level.GetTile(x + 1, y - 1) == Tile.InfiniteFall;
      bool flag8 = level.GetTile(x + 1, y + 1) == Tile.InfiniteFall;
      if (!flag1 && !flag3)
      {
        if (!flag5)
          screen.Render(x * 16, y * 16, 17, this.col, 0);
        else
          screen.Render(x * 16, y * 16, 7, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16, y * 16, (flag3 ? 6 : 5) + (flag1 ? 2 : 1) * 32, this.transitionColor, 3);
      if (!flag1 && !flag4)
      {
        if (!flag7)
          screen.Render(x * 16 + 8, y * 16, 18, this.col, 0);
        else
          screen.Render(x * 16 + 8, y * 16, 8, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16 + 8, y * 16, (flag4 ? 4 : 5) + (flag1 ? 2 : 1) * 32, this.transitionColor, 3);
      if (!flag2 && !flag3)
      {
        if (!flag6)
          screen.Render(x * 16, y * 16 + 8, 20, this.col, 0);
        else
          screen.Render(x * 16, y * 16 + 8, 39, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16, y * 16 + 8, (flag3 ? 6 : 5) + (flag2 ? 0 : 1) * 32, this.transitionColor, 3);
      if (!flag2 && !flag4)
      {
        if (!flag8)
          screen.Render(x * 16 + 8, y * 16 + 8, 19, this.col, 0);
        else
          screen.Render(x * 16 + 8, y * 16 + 8, 40, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16 + 8, y * 16 + 8, (flag4 ? 4 : 5) + (flag2 ? 0 : 1) * 32, this.transitionColor, 3);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => true;

    public override bool Interact(
      Level level,
      int xt,
      int yt,
      Player player,
      Item item,
      int attackDir)
    {
      if (!(item is ToolItem) || ((ToolItem) item).MyType != ToolType.Shovel || !player.PayStamina(5))
        return false;
      int num = this.random.Next(2) + 1;
      for (int index = 0; index < num; ++index)
        level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Cloud), xt * 16 + (this.random.Next(10) + 3), yt * 16 + (this.random.Next(10) + 3)));
      return true;
    }
  }
}
