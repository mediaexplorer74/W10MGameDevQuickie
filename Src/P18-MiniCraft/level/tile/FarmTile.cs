// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.FarmTile
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using GameManager.item;

#nullable disable
namespace GameManager.level.tile
{
  public class FarmTile : Tile
  {
    private int col;

    public FarmTile(int id)
      : base(id)
    {
      this.col = Color.Get(Level.DirtColor - 121, Level.DirtColor - 11, Level.DirtColor, Level.DirtColor + 111);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      screen.Render(x * 16, y * 16, 34, this.col, 1);
      screen.Render(x * 16 + 8, y * 16, 34, this.col, 0);
      screen.Render(x * 16, y * 16 + 8, 34, this.col, 0);
      screen.Render(x * 16 + 8, y * 16 + 8, 34, this.col, 1);
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

    public override void Tick(Level level, int xt, int yt)
    {
      int data = level.GetData(xt, yt);
      if (data >= 5)
        return;
      level.SetData(xt, yt, data + 1);
    }

    public override void SteppedOn(Level level, int xt, int yt, Entity entity)
    {
      if (this.random.Next(60) != 0 || level.GetData(xt, yt) < 5)
        return;
      level.SetTile(xt, yt, Tile.Dirt, 0);
    }
  }
}
