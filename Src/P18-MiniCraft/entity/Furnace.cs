// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Furnace


using GameManager.crafting;
using GameManager.gfx;
using GameManager.screen;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Furnace : Furniture
  {
    public Furnace()
      : base(nameof (Furnace))
    {
      this.Col = gfx.Color.Get(-1, 0, 222, 333);
      this.Sprite = 3;
      this.Xr = 3;
      this.Yr = 2;
    }

    public Furnace(Game1 game, BinaryReader reader)
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
      player.MyGame.SetMenu((Menu) new CraftingMenu(Crafting.CraftingType.furnace, player));
      return true;
    }

    public override bool CheckUse(Player player, int attackDir) => true;
  }
}
