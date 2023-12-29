// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.HardRockTile
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
  public class HardRockTile : Tile
  {
    private int col;
    private int transitionColor;

    public HardRockTile(int id)
      : base(id)
    {
      this.col = Color.Get(334, 334, 223, 223);
      this.transitionColor = Color.Get(1, 334, 445, Level.DirtColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      bool flag1 = level.GetTile(x, y - 1) != this;
      bool flag2 = level.GetTile(x, y + 1) != this;
      bool flag3 = level.GetTile(x - 1, y) != this;
      bool flag4 = level.GetTile(x + 1, y) != this;
      bool flag5 = level.GetTile(x - 1, y - 1) != this;
      bool flag6 = level.GetTile(x - 1, y + 1) != this;
      bool flag7 = level.GetTile(x + 1, y - 1) != this;
      bool flag8 = level.GetTile(x + 1, y + 1) != this;
      if (!flag1 && !flag3)
      {
        if (!flag5)
          screen.Render(x * 16, y * 16, 0, this.col, 0);
        else
          screen.Render(x * 16, y * 16, 7, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16, y * 16, (flag3 ? 6 : 5) + (flag1 ? 2 : 1) * 32, this.transitionColor, 3);
      if (!flag1 && !flag4)
      {
        if (!flag7)
          screen.Render(x * 16 + 8, y * 16, 1, this.col, 0);
        else
          screen.Render(x * 16 + 8, y * 16, 8, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16 + 8, y * 16, (flag4 ? 4 : 5) + (flag1 ? 2 : 1) * 32, this.transitionColor, 3);
      if (!flag2 && !flag3)
      {
        if (!flag6)
          screen.Render(x * 16, y * 16 + 8, 2, this.col, 0);
        else
          screen.Render(x * 16, y * 16 + 8, 39, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16, y * 16 + 8, (flag3 ? 6 : 5) + (flag2 ? 0 : 1) * 32, this.transitionColor, 3);
      if (!flag2 && !flag4)
      {
        if (!flag8)
          screen.Render(x * 16 + 8, y * 16 + 8, 3, this.col, 0);
        else
          screen.Render(x * 16 + 8, y * 16 + 8, 40, this.transitionColor, 3);
      }
      else
        screen.Render(x * 16 + 8, y * 16 + 8, (flag4 ? 4 : 5) + (flag2 ? 0 : 1) * 32, this.transitionColor, 3);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => false;

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
      this.hurt(level, x, y, 0);
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
        if (toolItem.MyType == ToolType.Pickaxe && toolItem.ToolLevel == 4 && player.PayStamina(4 - toolItem.ToolLevel))
        {
          this.hurt(level, xt, yt, this.random.Next(10) + toolItem.ToolLevel * 5 + 10);
          return true;
        }
      }
      return false;
    }

    public void hurt(Level level, int x, int y, int dmg)
    {
      int val = level.GetData(x, y) + dmg;
      level.Add((Entity) new SmashParticle(x * 16 + 8, y * 16 + 8));
      level.Add((Entity) new TextParticle(string.Empty + (object) dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));
      if (val >= 200)
      {
        int num1 = this.random.Next(4) + 1;
        for (int index = 0; index < num1; ++index)
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Stone), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
        int num2 = this.random.Next(2);
        for (int index = 0; index < num2; ++index)
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Coal), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
        level.SetTile(x, y, Tile.Dirt, 0);
      }
      else
        level.SetData(x, y, val);
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
