// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Workbench


using GameManager.crafting;
using GameManager.gfx;
using GameManager.screen;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Workbench : Furniture
  {
    public Workbench()
      : base(nameof (Workbench))
    {
      this.Col = gfx.Color.Get(-1, 100, 321, 431);
      this.Sprite = 4;
      this.Xr = 3;
      this.Yr = 2;
    }

    public Workbench(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
    }

    public override bool Use(Player player, int attackDir)
    {
      player.MyGame.SetMenu((Menu) new CraftingMenu(Crafting.CraftingType.workbench, player));
      return true;
    }

    public override bool CheckUse(Player player, int attackDir) => true;
  }
}
