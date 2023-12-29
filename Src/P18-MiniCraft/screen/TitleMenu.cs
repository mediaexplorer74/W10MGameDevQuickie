// Decompiled with JetBrains decompiler
// Type: GameManager.screen.TitleMenu


using GameManager.gfx;
using GameManager.sound;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class TitleMenu : Menu
  {
    private static string[] options = new string[5]
    {
      "Start game",
      "Load game",
      "Options",
      "How to play",
      "About"
    };
    private int selected;

    public TitleMenu()
    {
    }

    public TitleMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
      if (InputHandler.Up.Clicked)
        --this.selected;
      if (InputHandler.Down.Clicked)
        ++this.selected;
      int length = TitleMenu.options.Length;
      if (this.selected < 0)
        this.selected += length;
      if (this.selected >= length)
        this.selected -= length;
      if (!InputHandler.Attack.Clicked && !InputHandler.Menu.Clicked)
        return;
      switch (this.selected)
      {
        case 0:
          Sound.Test.Play();
          this.game.ResetGame();
          this.game.SetMenu((Menu) null);
          break;
        case 1:
          this.game.SetMenu((Menu) new LoadSaveMenu(this.game, (Menu) this, true));
          break;
        case 2:
          this.game.SetMenu((Menu) new OptionsMenu(this.game, (Menu) this));
          break;
        case 3:
          this.game.SetMenu((Menu) new InstructionsMenu((Menu) this));
          break;
        case 4:
          this.game.SetMenu((Menu) new AboutMenu((Menu) this));
          break;
      }
    }

    public override void Render(Screen screen)
    {
      screen.Clear(0);
      int num1 = 2;
      int num2 = 13;
      int colors = Color.Get(0, 10, 131, 551);
      int num3 = (screen.W - num2 * 8) / 2;
      int num4 = 24;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        for (int index2 = 0; index2 < num2; ++index2)
          screen.Render(num3 + index2 * 8, num4 + index1 * 8, index2 + (index1 + 6) * 32, colors, 0);
      }
      for (int index = 0; index < TitleMenu.options.Length; ++index)
      {
        string msg = TitleMenu.options[index];
        int col = Color.Get(0, 222, 222, 222);
        if (index == this.selected)
        {
          msg = "> " + msg + " <";
          col = Color.Get(0, 555, 555, 555);
        }
        Font.Draw(msg, screen, (8 + index) * 8, col);
      }
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
