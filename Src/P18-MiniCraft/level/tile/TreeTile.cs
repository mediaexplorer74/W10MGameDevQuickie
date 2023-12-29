// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.TreeTile
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
  public class TreeTile : Tile
  {
    private int col;
    private int barkCol1;
    private int barkCol2;

    public TreeTile(int id)
      : base(id)
    {
      this.ConnectsToGrass = true;
      this.col = Color.Get(10, 30, 151, Level.GrassColor);
      this.barkCol1 = Color.Get(10, 30, 430, Level.GrassColor);
      this.barkCol2 = Color.Get(10, 30, 320, Level.GrassColor);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      bool flag1 = level.GetTile(x, y - 1) == this;
      bool flag2 = level.GetTile(x - 1, y) == this;
      bool flag3 = level.GetTile(x + 1, y) == this;
      bool flag4 = level.GetTile(x, y + 1) == this;
      bool flag5 = level.GetTile(x - 1, y - 1) == this;
      bool flag6 = level.GetTile(x + 1, y - 1) == this;
      bool flag7 = level.GetTile(x - 1, y + 1) == this;
      bool flag8 = level.GetTile(x + 1, y + 1) == this;
      if (flag1 && flag5 && flag2)
        screen.Render(x * 16, y * 16, 42, this.col, 0);
      else
        screen.Render(x * 16, y * 16, 9, this.col, 0);
      if (flag1 && flag6 && flag3)
        screen.Render(x * 16 + 8, y * 16, 74, this.barkCol2, 0);
      else
        screen.Render(x * 16 + 8, y * 16, 10, this.col, 0);
      if (flag4 && flag7 && flag2)
        screen.Render(x * 16, y * 16 + 8, 74, this.barkCol2, 0);
      else
        screen.Render(x * 16, y * 16 + 8, 41, this.barkCol1, 0);
      if (flag4 && flag8 && flag3)
        screen.Render(x * 16 + 8, y * 16 + 8, 42, this.col, 0);
      else
        screen.Render(x * 16 + 8, y * 16 + 8, 106, this.barkCol2, 0);
    }

    public override void Tick(Level level, int xt, int yt)
    {
      int data = level.GetData(xt, yt);
      if (data <= 0)
        return;
      level.SetData(xt, yt, data - 1);
    }

    public override bool MayPass(Level level, int x, int y, Entity e) => false;

    public override void Hurt(Level level, int x, int y, Mob source, int dmg, int attackDir)
    {
      this.hurt(level, x, y, dmg);
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
        if (toolItem.MyType == ToolType.Axe && player.PayStamina(4 - toolItem.ToolLevel))
        {
          this.hurt(level, xt, yt, this.random.Next(10) + toolItem.ToolLevel * 5 + 10);
          return true;
        }
      }
      return false;
    }

    private void hurt(Level level, int x, int y, int dmg)
    {
      int num1 = this.random.Next(10) == 0 ? 1 : 0;
      for (int index = 0; index < num1; ++index)
        level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Apple), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
      int val = level.GetData(x, y) + dmg;
      level.Add((Entity) new SmashParticle(x * 16 + 8, y * 16 + 8));
      level.Add((Entity) new TextParticle(string.Empty + (object) dmg, x * 16 + 8, y * 16 + 8, Color.Get(-1, 500, 500, 500)));
      if (val >= 20)
      {
        int num2 = this.random.Next(2) + 1;
        for (int index = 0; index < num2; ++index)
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Wood), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
        int num3 = this.random.Next(this.random.Next(4) + 1);
        for (int index = 0; index < num3; ++index)
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Acorn), x * 16 + this.random.Next(10) + 3, y * 16 + this.random.Next(10) + 3));
        level.SetTile(x, y, Tile.Grass, 0);
      }
      else
        level.SetData(x, y, val);
    }
  }
}
