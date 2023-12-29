// Decompiled with JetBrains decompiler
// Type: GameManager.screen.InstructionsMenu


using GameManager.gfx;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class InstructionsMenu : Menu
  {
    public Menu Parent;

        public InstructionsMenu(Menu parent)
        {
            this.Parent = parent;
        }

        public InstructionsMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
      if (!InputHandler.Attack.Clicked && !InputHandler.Menu.Clicked)
        return;
      this.game.SetMenu(this.Parent);
    }

    public override void Render(Screen screen)
    {
      screen.Clear(0);
      Font.Draw("HOW TO PLAY", screen, 8, Color.Get(0, 555, 555, 555));
      Font.Draw("Move your character", screen, 24, Color.Get(0, 333, 333, 333));
      Font.Draw("with the arrow keys", screen, 32, Color.Get(0, 333, 333, 333));
      Font.Draw("press C to attack", screen, 40, Color.Get(0, 333, 333, 333));
      Font.Draw("and X to open the", screen, 48, Color.Get(0, 333, 333, 333));
      Font.Draw("inventory and to", screen, 56, Color.Get(0, 333, 333, 333));
      Font.Draw("use items.", screen, 64, Color.Get(0, 333, 333, 333));
      Font.Draw("Select an item in", screen, 72, Color.Get(0, 333, 333, 333));
      Font.Draw("the inventory to", screen, 80, Color.Get(0, 333, 333, 333));
      Font.Draw("equip it.", screen, 88, Color.Get(0, 333, 333, 333));
      Font.Draw("Kill the air wizard", screen, 96, Color.Get(0, 333, 333, 333));
      Font.Draw("to win the game!", screen, 104, Color.Get(0, 333, 333, 333));
    }

    public override void SaveToWriter(BinaryWriter writer)
    {
      base.SaveToWriter(writer);
      this.Parent.SaveToWriter(writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.Parent = Menu.NewMenuFromReader(game, reader);
    }
  }
}
