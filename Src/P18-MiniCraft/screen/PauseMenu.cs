// Decompiled with JetBrains decompiler
// Type: GameManager.screen.PauseMenu

using GameManager.gfx;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class PauseMenu : Menu
  {
    private static string[] options = new string[5]
    {
      "Resume",
      "Load",
      "Save",
      "Options",
      "Quit"
    };
    private int selected;

    public PauseMenu(Game1 game)
    {
      this.Init(game, game.Input);
      this.selected = 0;
    }

    public PauseMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.LoadFromReader(game, reader);
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

    public override void Tick()
    {
      int length = PauseMenu.options.Length;

      if (InputHandler.Up.Clicked)
      {
        --this.selected;
        if (this.selected < 0)
          this.selected += length;
      }

      if (InputHandler.Down.Clicked)
      {
        ++this.selected;
        if (this.selected >= length)
          this.selected -= length;
      }

      if (InputHandler.Menu.Clicked)
      {
        this.game.SetMenu((Menu) null);
      }
      else
      {
        if (!InputHandler.Attack.Clicked)
          return;
        switch (this.selected)
        {
          case 0:
            this.game.SetMenu((Menu) null);
            break;
          case 1:
            this.game.SetMenu((Menu) new LoadSaveMenu(this.game, (Menu) this, true));
            break;
          case 2:
            this.game.SetMenu((Menu) new LoadSaveMenu(this.game, (Menu) this, false));
            break;
          case 3:
            this.game.SetMenu((Menu) new OptionsMenu(this.game, (Menu) this));
            break;
          case 4:
            this.game.SetMenu((Menu) new TitleMenu());
            break;
        }
      }
    }

    public override void Render(Screen screen)
    {
      if (miniprefs.ScreenSize == 0)
        Font.RenderFrame(screen, "PAUSE", screen.W / 16 - 6, screen.H / 16 - 4, screen.W / 16 + 6, screen.H / 16 + 4);
      else
        Font.RenderFrame(screen, "PAUSE", screen.W / 16 - 6, screen.H / 16 - 4, screen.W / 16 + 5, screen.H / 16 + 4);
      for (int index = 0; index < PauseMenu.options.Length; ++index)
      {
        string msg = PauseMenu.options[index];
        int col = Color.Get(5, 222, 222, 222);
        if (index == this.selected)
        {
          msg = "> " + msg + " <";
          col = Color.Get(5, 555, 555, 555);
        }
        Font.Draw(msg, screen, screen.H / 2 - 16 + index * 8, col);
      }
    }
  }
}
