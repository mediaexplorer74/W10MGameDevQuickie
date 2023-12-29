// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Oven


using GameManager.crafting;
using GameManager.gfx;
using GameManager.screen;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Oven : Furniture
  {
    public Oven()
      : base(nameof (Oven))
    {
      this.Col = gfx.Color.Get(-1, 0, 332, 442);
      this.Sprite = 2;
      this.Xr = 3;
      this.Yr = 2;
    }

    public Oven(Game1 game, BinaryReader reader)
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
      player.MyGame.SetMenu((Menu) new CraftingMenu(Crafting.CraftingType.oven, player));
      return true;
    }

    public override bool CheckUse(Player player, int attackDir) => true;
  }
}
