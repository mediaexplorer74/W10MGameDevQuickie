// Decompiled with JetBrains decompiler
// Type: GameManager.screen.OptionsMenu


using GameManager.gfx;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class OptionsMenu : Menu
  {
    private static string[] options = new string[4]
    {
      "Sound",
      "Screen",
      "Autosave",
      "Minimap"
    };
    public Menu Parent;
    private int selected;

    public OptionsMenu(Game1 game, Menu parent)
    {
      this.Init(game, game.Input);
      this.Parent = parent;
      this.selected = 0;
    }

    public OptionsMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.LoadFromReader(game, reader);
    }

    public override void SaveToWriter(BinaryWriter writer)
    {
      base.SaveToWriter(writer);
      writer.Write(this.selected);
      this.Parent.SaveToWriter(writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.selected = reader.ReadInt32();
      this.Parent = Menu.NewMenuFromReader(game, reader);
    }

    public override void Tick()
    {
      int length = OptionsMenu.options.Length;

            //RnD
            //this.input.Up.....
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

      //RnD
      if (1==0)//(this.input.Menu.Clicked)
      {
        miniprefs.Save();
        this.game.Resize();
        this.game.SetMenu(this.Parent);
      }
      else
      {
                //RnD
        //if (!this.input.Attack.Clicked)
          return;
        switch (this.selected)
        {
          case 0:
            miniprefs.UseSound = !miniprefs.UseSound;
            break;
          case 1:
            ++miniprefs.ScreenSize;
            break;
          case 2:
            miniprefs.Autosave = !miniprefs.Autosave;
            break;
          case 3:
            miniprefs.Minimap = !miniprefs.Minimap;
            break;
        }
        
        //RnD
        //this.game.OnClearInput();
      }
    }

    public override void Render(Screen screen)
    {
      screen.Clear(0);
      Font.Draw("OPTIONS", screen, 8, Color.Get(0, 555, 555, 555));
      for (int index = 0; index < OptionsMenu.options.Length; ++index)
      {
        string msg = OptionsMenu.options[index] + ": ";
        switch (index)
        {
          case 0:
            msg += miniprefs.UseSoundString;
            break;
          case 1:
            msg += miniprefs.ScreenSizeString;
            break;
          case 2:
            msg += miniprefs.AutosaveString;
            break;
          case 3:
            msg += miniprefs.MinimapString;
            break;
        }
        int col = Color.Get(0, 222, 222, 222);
        if (index == this.selected)
        {
          msg = "> " + msg + " <";
          col = Color.Get(0, 555, 555, 555);
        }
        Font.Draw(msg, screen, (8 + index) * 8, col);
      }
    }
  }
}
