// Decompiled with JetBrains decompiler
// Type: GameManager.gfx.Color
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

#nullable disable
namespace GameManager.gfx
{
  public static class Color
  {
    public static int Get(int a, int b, int c, int d)
    {
      return (Color.Get(d) << 24) + (Color.Get(c) << 16) + (Color.Get(b) << 8) + Color.Get(a);
    }

    public static int Get(int d)
    {
      return d < 0 ? (int) byte.MaxValue : d / 100 % 10 * 36 + d / 10 % 10 * 6 + d % 10;
    }
  }
}
