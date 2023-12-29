// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.GrassTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using GameManager.item;
using GameManager.item.resource;
using GameManager.sound;

#nullable disable
namespace GameManager.level.tile
{
  public class GrassTile : Tile
  {
    private int col;
    private int transitionColor;

    public GrassTile(int id)
      : base(id)
    {
      this.ConnectsToGrass = true;
      this.col = Color.Get(Level.GrassColor, Level.GrassColor, Level.GrassColor + 111, Level.GrassColor + 111);
      this.transitionColor = Color.Get(Level.GrassColor - 111, Level.GrassColor, Level.GrassColor + 111, Level.DirtColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      bool flag1 = !level.GetTile(x, y - 1).ConnectsToGrass;
      bool flag2 = !level.GetTile(x, y + 1).ConnectsToGrass;
      bool flag3 = !level.GetTile(x - 1, y).ConnectsToGrass;
      bool flag4 = !level.GetTile(x + 1, y).ConnectsToGrass;
      if (!flag1 && !flag3)
        screen.Render(x * 16, y * 16, 0, this.col, 0);
      else
        screen.Render(x * 16, y * 16, (flag3 ? 11 : 12) + (flag1 ? 0 : 1) * 32, this.transitionColor, 0);
      if (!flag1 && !flag4)
        screen.Render(x * 16 + 8, y * 16, 1, this.col, 0);
      else
        screen.Render(x * 16 + 8, y * 16, (flag4 ? 13 : 12) + (flag1 ? 0 : 1) * 32, this.transitionColor, 0);
      if (!flag2 && !flag3)
        screen.Render(x * 16, y * 16 + 8, 2, this.col, 0);
      else
        screen.Render(x * 16, y * 16 + 8, (flag3 ? 11 : 12) + (flag2 ? 2 : 1) * 32, this.transitionColor, 0);
      if (!flag2 && !flag4)
        screen.Render(x * 16 + 8, y * 16 + 8, 3, this.col, 0);
      else
        screen.Render(x * 16 + 8, y * 16 + 8, (flag4 ? 13 : 12) + (flag2 ? 2 : 1) * 32, this.transitionColor, 0);
    }

    public override void Tick(Level level, int xt, int yt)
    {
      if (this.random.Next(40) != 0)
        return;
      int x = xt;
      int y = yt;
      if (this.random.Next(2) == 0)
        x += this.random.Next(2) * 2 - 1;
      else
        y += this.random.Next(2) * 2 - 1;
      if (level.GetTile(x, y) != Tile.Dirt)
        return;
      level.SetTile(x, y, (Tile) this, 0);
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
        if (toolItem.MyType == ToolType.Shovel && player.PayStamina(4 - toolItem.ToolLevel))
        {
          level.SetTile(xt, yt, Tile.Dirt, 0);
          Sound.MonsterHurt.Play();
          if (this.random.Next(5) == 0)
          {
            level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Seeds), xt * 16 + this.random.Next(10) + 3, yt * 16 + this.random.Next(10) + 3));
            return true;
          }
        }
        if (toolItem.MyType == ToolType.Hoe && player.PayStamina(4 - toolItem.ToolLevel))
        {
          Sound.MonsterHurt.Play();
          if (this.random.Next(5) == 0)
          {
            level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Seeds), xt * 16 + this.random.Next(10) + 3, yt * 16 + this.random.Next(10) + 3));
            return true;
          }
          level.SetTile(xt, yt, Tile.Farmland, 0);
          return true;
        }
      }
      return false;
    }
  }
}
