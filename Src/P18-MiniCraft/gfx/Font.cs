// Decompiled with JetBrains decompiler
// Type: GameManager.gfx.Font
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using System.Globalization;

#nullable disable
namespace GameManager.gfx
{
  public static class Font
  {
    private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ      0123456789.,!?'\"-+=/\\%()<>:;     ";

    public static void Draw(string msg, Screen screen, int x, int y, int col)
    {
      msg = msg.ToUpper();// CultureInfo.CurrentCulture);
      for (int index = 0; index < msg.Length; ++index)
      {
        int num = Font.chars.IndexOf(msg[index]);
        if (num >= 0)
          screen.Render(x + index * 8, y, num + 960, col, 0);
      }
    }

    public static void Draw(string msg, Screen screen, int y, int col)
    {
      int num1 = screen.W / 2 - msg.Length * 4;
            
      msg = msg.ToUpper();// CultureInfo.CurrentCulture);
      for (int index = 0; index < msg.Length; ++index)
      {
        int num2 = Font.chars.IndexOf(msg[index]);
        if (num2 >= 0)
          screen.Render(num1 + index * 8, y, num2 + 960, col, 0);
      }
    }

    public static void RenderFrame(Screen screen, string title, int x0, int y0, int x1, int y1)
    {
      for (int index1 = y0; index1 <= y1; ++index1)
      {
        for (int index2 = x0; index2 <= x1; ++index2)
        {
          if (index2 == x0 && index1 == y0)
            screen.Render(index2 * 8, index1 * 8, 416, Color.Get(-1, 1, 5, 445), 0);
          else if (index2 == x1 && index1 == y0)
            screen.Render(index2 * 8, index1 * 8, 416, Color.Get(-1, 1, 5, 445), 1);
          else if (index2 == x0 && index1 == y1)
            screen.Render(index2 * 8, index1 * 8, 416, Color.Get(-1, 1, 5, 445), 2);
          else if (index2 == x1 && index1 == y1)
            screen.Render(index2 * 8, index1 * 8, 416, Color.Get(-1, 1, 5, 445), 3);
          else if (index1 == y0)
            screen.Render(index2 * 8, index1 * 8, 417, Color.Get(-1, 1, 5, 445), 0);
          else if (index1 == y1)
            screen.Render(index2 * 8, index1 * 8, 417, Color.Get(-1, 1, 5, 445), 2);
          else if (index2 == x0)
            screen.Render(index2 * 8, index1 * 8, 418, Color.Get(-1, 1, 5, 445), 0);
          else if (index2 == x1)
            screen.Render(index2 * 8, index1 * 8, 418, Color.Get(-1, 1, 5, 445), 1);
          else
            screen.Render(index2 * 8, index1 * 8, 418, Color.Get(5, 5, 5, 5), 1);
        }
      }
      Font.Draw(title, screen, x0 * 8 + 8, y0 * 8, Color.Get(5, 5, 5, 550));
    }
  }
}
