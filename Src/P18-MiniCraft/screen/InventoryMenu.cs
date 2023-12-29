// Decompiled with JetBrains decompiler
// Type: GameManager.screen.InventoryMenu


using GameManager.entity;
using GameManager.gfx;
using GameManager.item;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class InventoryMenu : Menu
  {
    private Player player;
    private int selected;

    public InventoryMenu(Player player)
    {
      this.player = player;
      if (player.ActiveItem == null)
        return;
      player.PlayerInventory.Items.Insert(0, (ListItem) player.ActiveItem);
      player.ActiveItem = (Item) null;
    }

    public InventoryMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.player = game.ActivePlayer;
      this.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
      if (InputHandler.Menu.Clicked)
        this.game.SetMenu((Menu) null);
      if (InputHandler.Up.Clicked)
        --this.selected;
      if (InputHandler.Down.Clicked)
        ++this.selected;

      int count = this.player.PlayerInventory.Items.Count;
      if (count == 0)
        this.selected = 0;
      if (this.selected < 0)
        this.selected += count;
      if (this.selected >= count)
        this.selected -= count;

      if (!InputHandler.Attack.Clicked || count <= 0)
        return;
      Item obj = (Item) this.player.PlayerInventory.Items[this.selected];
      this.player.PlayerInventory.Items.RemoveAt(this.selected);
      this.player.ActiveItem = obj;
      this.game.SetMenu((Menu) null);
    }

    public override void Render(Screen screen)
    {
      Font.RenderFrame(screen, "inventory", 1, 1, 12, 11);
      this.RenderItemList(screen, 1, 1, 12, 11, this.player.PlayerInventory.Items, this.selected);
    }

    public override void SaveToWriter(BinaryWriter writer)
    {
      base.SaveToWriter(writer);
      writer.Write(this.selected);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.selected = reader.ReadInt32();
    }
  }
}
