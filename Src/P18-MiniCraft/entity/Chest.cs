// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Chest


using GameManager.gfx;
using GameManager.screen;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Chest : Furniture
  {
    public Inventory ChestInventory = new Inventory();

    public Chest()
      : base(nameof (Chest))
    {
      this.Col = gfx.Color.Get(-1, 110, 331, 552);
      this.Sprite = 1;
    }

    public Chest(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      this.ChestInventory.SaveToWriter(game, writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.ChestInventory = new Inventory(game, reader);
    }

    public override bool Use(Player player, int attackDir)
    {
      player.MyGame.SetMenu((Menu) new ContainerMenu(player, nameof (Chest), this.ChestInventory));
      return true;
    }

    public override bool CheckUse(Player player, int attackDir) => true;
  }
}
