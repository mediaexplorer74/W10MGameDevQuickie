// Decompiled with JetBrains decompiler
// Type: GameManager.screen.LoadSaveMenu


using GameManager.gfx;
using System;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class LoadSaveMenu : Menu
  {
    private static string[] saves = new string[5]
    {
      "q",
      "1",
      "2",
      "3",
      "a"
    };
    private static DateTime[] saveDates = new DateTime[5];
    public Menu Parent;
    private int selected;
    private bool doLoad;

    public LoadSaveMenu(Game1 game, Menu parent, bool doLoad)
    {
      this.Init(game, game.Input);
      this.Parent = parent;
      this.doLoad = doLoad;
      this.selected = !doLoad ? 1 : 0;
      this.GetSaveDates();
    }

    public LoadSaveMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.LoadFromReader(game, reader);
      this.GetSaveDates();
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

    private void GetSaveDates()
    {
      for (int index = 0; index < LoadSaveMenu.saves.Length; ++index)
        LoadSaveMenu.saveDates[index] = this.game.GetSaveDate(index);
    }

    public override void Tick()
    {
      int length = LoadSaveMenu.saves.Length;
      if (InputHandler.Up.Clicked)
      {
        --this.selected;
        if (this.selected < 0)
          this.selected += length;
        if (!this.doLoad)
        {
          if (this.selected == 0)
            this.selected = length - 2;
          else if (this.selected == length - 1)
            this.selected = 1;
        }
      }
      if (InputHandler.Down.Clicked)
      {
        ++this.selected;
        if (this.selected >= length)
          this.selected -= length;
        if (!this.doLoad && (this.selected == 0 || this.selected == length - 1))
          this.selected = 1;
      }
      if (InputHandler.Menu.Clicked)
      {
        this.game.SetMenu(this.Parent);
      }
      else
      {
        if (!InputHandler.Attack.Clicked)
          return;
        if (this.doLoad)
        {
          if (!(LoadSaveMenu.saveDates[this.selected] > DateTime.MinValue))
            return;
          this.game.LoadGame(this.selected);
        }
        else
        {
          this.game.SaveGame(this.selected);
          this.game.SetMenu((Menu) null);
        }
      }
    }

    public override void Render(Screen screen)
    {
      screen.Clear(0);
      if (this.doLoad)
        Font.Draw("LOAD GAME", screen, 8, Color.Get(0, 555, 555, 555));
      else
        Font.Draw("SAVE GAME", screen, 8, Color.Get(0, 555, 555, 555));
      int y = 48;
      for (int index = 0; index < LoadSaveMenu.saves.Length; ++index)
      {
        string str = LoadSaveMenu.saves[index] + " ";
        string msg = !(LoadSaveMenu.saveDates[index] > DateTime.MinValue) 
                    ? str + "<empty>" 
                    : str + LoadSaveMenu.saveDates[index].ToString() +
                    " " + LoadSaveMenu.saveDates[index].ToString();
        int col = Color.Get(0, 222, 222, 222);
        int num;
        if (index == this.selected)
        {
          msg = "> " + msg + " <";
          col = Color.Get(0, 555, 555, 555);
          num = 25;
        }
        else
          num = 21;
        Font.Draw(msg, screen, (screen.W - num * 8) / 2, y, col);
        if (index == 0 || index == 3)
          y += 16;
        else
          y += 12;
      }
    }
  }
}
