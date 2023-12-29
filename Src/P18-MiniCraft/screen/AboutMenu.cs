// Decompiled with JetBrains decompiler
// Type: GameManager.screen.AboutMenu
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.gfx;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class AboutMenu : Menu
  {
    public Menu Parent;

        public AboutMenu(Menu parent)
        {
            this.Parent = parent;
        }

        public AboutMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, input);
      this.LoadFromReader(game, reader);
    }

    public override void Tick()
    {
      //RnD
      if (!InputHandler.Attack.Clicked && !InputHandler.Menu.Clicked)
       return;
     
      this.game.SetMenu(this.Parent);
    }

    public override void Render(Screen screen)
    {
      screen.Clear(0);
      Font.Draw("About Minicraft", screen, 8, gfx.Color.Get(0, 555, 555, 555));
      Font.Draw("Minicraft was made", screen, 24, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("by Markus Persson", screen, 32, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("For the 22'nd ludum", screen, 40, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("dare competition in", screen, 48, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("december 2011.", screen, 56, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("it is dedicated to", screen, 64, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("my father. <3", screen, 72, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("Windows version by", screen, 88, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("Peter Elzner", screen, 96, gfx.Color.Get(0, 333, 333, 333));
      Font.Draw("www.diamond-pro.com", screen, 104, gfx.Color.Get(0, 333, 333, 333));
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
