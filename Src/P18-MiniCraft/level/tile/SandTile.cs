// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.SandTile
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
  public class SandTile : Tile
  {
    private int col;
    private int transitionColor;

    public SandTile(int id)
      : base(id)
    {
      this.ConnectsToSand = true;
      this.col = Color.Get(Level.SandColor + 2, Level.SandColor, Level.SandColor - 110, Level.SandColor - 110);
      this.transitionColor = Color.Get(Level.SandColor - 110, Level.SandColor, Level.SandColor - 110, Level.DirtColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      bool flag1 = !level.GetTile(x, y - 1).ConnectsToSand;
      bool flag2 = !level.GetTile(x, y + 1).ConnectsToSand;
      bool flag3 = !level.GetTile(x - 1, y).ConnectsToSand;
      bool flag4 = !level.GetTile(x + 1, y).ConnectsToSand;
      bool flag5 = level.GetData(x, y) > 0;
      if (!flag1 && !flag3)
      {
        if (!flag5)
          screen.Render(x * 16, y * 16, 0, this.col, 0);
        else
          screen.Render(x * 16, y * 16, 35, this.col, 0);
      }
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
      {
        if (!flag5)
          screen.Render(x * 16 + 8, y * 16 + 8, 3, this.col, 0);
        else
          screen.Render(x * 16 + 8, y * 16 + 8, 35, this.col, 0);
      }
      else
        screen.Render(x * 16 + 8, y * 16 + 8, (flag4 ? 13 : 12) + (flag2 ? 2 : 1) * 32, this.transitionColor, 0);
    }

    public override void Tick(Level level, int x, int y)
    {
      int data = level.GetData(x, y);
      if (data <= 0)
        return;
      level.SetData(x, y, data - 1);
    }

    public override void SteppedOn(Level level, int x, int y, Entity entity)
    {
      if (!(entity is Mob))
        return;
      level.SetData(x, y, 10);
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
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Sand), xt * 16 + this.random.Next(10) + 3, yt * 16 + this.random.Next(10) + 3));
          return true;
        }
      }
      return false;
    }
  }
}
