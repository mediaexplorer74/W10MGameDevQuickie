// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Anvil

using GameManager.crafting;
using GameManager.gfx;
using GameManager.screen;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Anvil : Furniture
  {
    public Anvil()
      : base(nameof (Anvil))
    {
      this.Col = gfx.Color.Get(-1, 0, 111, 222);
      this.Sprite = 0;
      this.Xr = 3;
      this.Yr = 2;
    }

    public Anvil(Game1 game, BinaryReader reader)
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
      player.MyGame.SetMenu((Menu) new CraftingMenu(Crafting.CraftingType.anvil, player));
      return true;
    }

    public override bool CheckUse(Player player, int attackDir) => true;
  }
}
