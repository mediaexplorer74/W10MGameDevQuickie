// Decompiled with JetBrains decompiler
// Type: GameManager.level.tile.DirtTile
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
  public class DirtTile : Tile
  {
    private int col;

    public DirtTile(int id)
      : base(id)
    {
      this.col = Color.Get(Level.DirtColor, Level.DirtColor, Level.DirtColor - 111, Level.DirtColor - 111);
    }

    public override void Render(Screen screen, Level level, int x, int y)
    {
      screen.Render(x * 16, y * 16, 0, this.col, 0);
      screen.Render(x * 16 + 8, y * 16, 1, this.col, 0);
      screen.Render(x * 16, y * 16 + 8, 2, this.col, 0);
      screen.Render(x * 16 + 8, y * 16 + 8, 3, this.col, 0);
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
          level.SetTile(xt, yt, Tile.Hole, 0);
          level.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Dirt), xt * 16 + this.random.Next(10) + 3, yt * 16 + this.random.Next(10) + 3));
          Sound.MonsterHurt.Play();
          return true;
        }
        if (toolItem.MyType == ToolType.Hoe && player.PayStamina(4 - toolItem.ToolLevel))
        {
          level.SetTile(xt, yt, Tile.Farmland, 0);
          Sound.MonsterHurt.Play();
          return true;
        }
      }
      return false;
    }
  }
}
