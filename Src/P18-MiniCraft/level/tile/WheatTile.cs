// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.WheatTile
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
  public class WheatTile : Tile
  {
    public WheatTile(int id)
      : base(id)
    {
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      int data = level.GetData(x, y);
      int num = data / 10;
      int colors;
      if (num >= 3)
      {
        colors = Color.Get(Level.DirtColor - 121, Level.DirtColor - 11, 50 + num * 100, 40 + (num - 3) * 2 * 100);
        if (data == 50)
          colors = Color.Get(0, 0, 50 + num * 100, 40 + (num - 3) * 2 * 100);
        num = 3;
      }
      else
        colors = Color.Get(Level.DirtColor - 121, Level.DirtColor - 11, Level.DirtColor, 50);
      screen.Render(x * 16, y * 16, 100 + num, colors, 0);
      screen.Render(x * 16 + 8, y * 16, 100 + num, colors, 0);
      screen.Render(x * 16, y * 16 + 8, 100 + num, colors, 1);
      screen.Render(x * 16 + 8, y * 16 + 8, 100 + num, colors, 1);
    }

    public override void Tick(Level level, int xt, int yt)
    {
      if (this.random.Next(2) == 0)
        return;
      int data = level.GetData(xt, yt);
      if (data >= 50)
        return;
      level.SetData(xt, yt, data + 1);
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
          return true;
        }
      }
      return false;
    }

    public override void SteppedOn(Level level, int xt, int yt, Entity entity)
    {
      if (this.random.Next(60) != 0 || level.GetData(xt, yt) < 2)
        return;
      this.harvest(level, xt, yt);
    }

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
      this.harvest(level, x, y);
    }

    private void harvest(Level level, int x, int y)
    {
      int data = level.GetData(x, y);
      int num1 = this.random.Next(2);
      for (int index = 0; index < num1; ++index)
        level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Seeds), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
      int num2 = 0;
      if (data == 50)
        num2 = this.random.Next(3) + 2;
      else if (data >= 40)
        num2 = this.random.Next(2) + 1;
      for (int index = 0; index < num2; ++index)
        level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Wheat), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
      level.SetTile(x, y, Tile.Dirt, 0);
    }
  }
}
