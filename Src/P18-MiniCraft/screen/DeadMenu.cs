// Decompiled with JetBrains decompiler
// Type: GameManager.screen.DeadMenu

using GameManager.gfx;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class DeadMenu : Menu
  {
    private int inputDelay = 60;

    public DeadMenu()
    {
    }

    public DeadMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
      if (this.inputDelay > 0)
      {
        --this.inputDelay;
      }
      else
      {
        if (!InputHandler.Attack.Clicked && !InputHandler.Menu.Clicked)
          return;
        this.game.SetMenu((Menu) new TitleMenu());
      }
    }

    public override void Render(Screen screen)
    {
      Font.RenderFrame(screen, string.Empty, 1, 3, 18, 9);
      Font.Draw("You died! Aww!", screen, 16, 32, Color.Get(-1, 555, 555, 555));
      int num1 = this.game.GameTime / 60;
      int num2 = num1 / 60;
      int num3 = num2 / 60;
      int num4 = num2 % 60;
      int num5 = num1 % 60;
      string empty = string.Empty;
      string msg;
      if (num3 > 0)
        msg = num3.ToString() + "h" + (num4 < 10 
                    ? (object) "0"
                    : (object) string.Empty) + (object) num4 + "m";
      else
        msg = num4.ToString() + "m " + (num5 < 10 
                    ? (object) "0" 
                    : (object) string.Empty) + (object) num5 + "s";
      Font.Draw("Time:", screen, 16, 40, Color.Get(-1, 555, 555, 555));
      Font.Draw(msg, screen, 56, 40, Color.Get(-1, 550, 550, 550));
      Font.Draw("Score:", screen, 16, 48, Color.Get(-1, 555, 555, 555));
      Font.Draw(string.Empty + (object) this.game.ActivePlayer.Score, screen, 
          64, 48, Color.Get(-1, 550, 550, 550));
    }

    public override void SaveToWriter(BinaryWriter writer)
    {
      base.SaveToWriter(writer);
      writer.Write(this.inputDelay);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.inputDelay = reader.ReadInt32();
    }
  }
}
