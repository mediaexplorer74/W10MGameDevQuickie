// Decompiled with JetBrains decompiler
// Type: GameManager.screen.ContainerMenu


using GameManager.entity;
using GameManager.gfx;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class ContainerMenu : Menu
  {
    private Player player;
    private Inventory container;
    private int selected;
    private string title;
    private int oSelected;
    private int window;

    public ContainerMenu(Player player, string title, Inventory container)
    {
      this.player = player;
      this.title = title;
      this.container = container;
    }

    public ContainerMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.player = game.ActivePlayer;
      this.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
      if (InputHandler.Menu.Clicked)
        this.game.SetMenu((Menu) null);

      //RnD
      if (InputHandler.Left.Clicked)
      {
        this.window = 0;
        int selected = this.selected;
        this.selected = this.oSelected;
        this.oSelected = selected;
      }
      if (InputHandler.Right.Clicked)
      {
        this.window = 1;
        int selected = this.selected;
        this.selected = this.oSelected;
        this.oSelected = selected;
      }
      Inventory inventory1 = this.window == 1 ? this.player.PlayerInventory : this.container;
      Inventory inventory2 = this.window == 0 ? this.player.PlayerInventory : this.container;
      int count = inventory1.Items.Count;
      if (count == 0)
      {
        this.selected = 0;
      }
      else
      {
        if (this.selected < 0)
          this.selected = 0;

        if (this.selected >= count)
          this.selected = count - 1;
        if (InputHandler.Up.Clicked)
          --this.selected;

        if (InputHandler.Down.Clicked)
          ++this.selected;

        while (this.selected < 0)
          this.selected += count;

        if (this.selected >= count)
          this.selected -= count;

        if (!InputHandler.Attack.Clicked)
          return;

        ListItem listItem = inventory1.Items[this.selected];
        inventory1.Items.RemoveAt(this.selected);
        inventory2.Add(this.oSelected, listItem);

        if (this.selected < inventory1.Items.Count)
          return;

        this.selected = inventory1.Items.Count - 1;
      }
    }

    public override void Render(Screen screen)
    {
      if (this.window == 1)
        screen.SetOffset(48, 0);
      Font.RenderFrame(screen, this.title, 1, 1, 12, 11);
      this.RenderItemList(screen, 1, 1, 12, 11, this.container.Items, this.window == 0
          ? this.selected : -this.oSelected - 1);
      Font.RenderFrame(screen, "inventory", 13, 1, 24, 11);

      this.RenderItemList(screen, 13, 1, 24, 11,
          this.player.PlayerInventory.Items, this.window == 1 ? this.selected : -this.oSelected - 1);
      screen.SetOffset(0, 0);
    }

    public override void SaveToWriter(BinaryWriter writer)
    {
      base.SaveToWriter(writer);
      writer.Write(this.selected);
      writer.Write(this.title);
      writer.Write(this.oSelected);
      writer.Write(this.window);
      this.container.SaveToWriter(this.game, writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.selected = reader.ReadInt32();
      this.title = reader.ReadString();
      this.oSelected = reader.ReadInt32();
      this.window = reader.ReadInt32();
      this.container = new Inventory(game, reader);
    }
  }
}
