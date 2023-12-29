// Decompiled with JetBrains decompiler
// Type: GameManager.gfx.Screen
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

#nullable disable
namespace GameManager.gfx
{
  public class Screen
  {
    public const int BIT_MIRROR_X = 1;
    public const int BIT_MIRROR_Y = 2;
    public readonly int W;
    public readonly int H;
    public int OffsetX;
    public int OffsetY;
    public int[] Pixels;
    private SpriteSheet sheet;
    private int sheetWidth;
    private int[] dither = new int[16]
    {
      0,
      8,
      2,
      10,
      12,
      4,
      14,
      6,
      3,
      11,
      1,
      9,
      15,
      7,
      13,
      5
    };

    public Screen(int w, int h, SpriteSheet sheet)
    {
      this.sheet = sheet;
      this.W = w;
      this.H = h;
      this.Pixels = new int[w * h];
      this.sheetWidth = sheet.width;
    }

    public void Clear(int color)
    {
      for (int index = 0; index < this.Pixels.Length; ++index)
        this.Pixels[index] = color;
    }

    public void Render(int xp, int yp, int tile, int colors, int bits)
    {
      xp -= this.OffsetX;
      yp -= this.OffsetY;
      bool flag1 = (bits & 1) > 0;
      bool flag2 = (bits & 2) > 0;
      int num1 = tile % 32 * 8 + tile / 32 * 8 * this.sheetWidth;
      if (flag2)
      {
        if (flag1)
        {
          for (int index1 = 0; index1 < 8; ++index1)
          {
            if (index1 + yp >= 0 && index1 + yp < this.H)
            {
              int num2 = (7 - index1) * this.sheetWidth + num1;
              for (int index2 = 0; index2 < 8; ++index2)
              {
                if (index2 + xp >= 0 && index2 + xp < this.W)
                {
                  int num3 = colors >> this.sheet.Pixels[7 - index2 + num2] * 8 & (int) byte.MaxValue;
                  if (num3 < (int) byte.MaxValue)
                    this.Pixels[index2 + xp + (index1 + yp) * this.W] = num3;
                }
              }
            }
          }
        }
        else
        {
          for (int index3 = 0; index3 < 8; ++index3)
          {
            if (index3 + yp >= 0 && index3 + yp < this.H)
            {
              int num4 = (7 - index3) * this.sheetWidth + num1;
              for (int index4 = 0; index4 < 8; ++index4)
              {
                if (index4 + xp >= 0 && index4 + xp < this.W)
                {
                  int num5 = colors >> this.sheet.Pixels[index4 + num4] * 8 & (int) byte.MaxValue;
                  if (num5 < (int) byte.MaxValue)
                    this.Pixels[index4 + xp + (index3 + yp) * this.W] = num5;
                }
              }
            }
          }
        }
      }
      else if (flag1)
      {
        for (int index5 = 0; index5 < 8; ++index5)
        {
          if (index5 + yp >= 0 && index5 + yp < this.H)
          {
            int num6 = index5 * this.sheetWidth + num1;
            for (int index6 = 0; index6 < 8; ++index6)
            {
              if (index6 + xp >= 0 && index6 + xp < this.W)
              {
                int num7 = colors >> this.sheet.Pixels[7 - index6 + num6] * 8 & (int) byte.MaxValue;
                if (num7 < (int) byte.MaxValue)
                  this.Pixels[index6 + xp + (index5 + yp) * this.W] = num7;
              }
            }
          }
        }
      }
      else
      {
        for (int index7 = 0; index7 < 8; ++index7)
        {
          if (index7 + yp >= 0 && index7 + yp < this.H)
          {
            int num8 = index7 * this.sheetWidth + num1;
            for (int index8 = 0; index8 < 8; ++index8)
            {
              if (index8 + xp >= 0 && index8 + xp < this.W)
              {
                int num9 = colors >> this.sheet.Pixels[index8 + num8] * 8 & (int) byte.MaxValue;
                if (num9 < (int) byte.MaxValue)
                  this.Pixels[index8 + xp + (index7 + yp) * this.W] = num9;
              }
            }
          }
        }
      }
    }

    public void SetOffset(int xOffset, int yOffset)
    {
      this.OffsetX = xOffset;
      this.OffsetY = yOffset;
    }

    public void Overlay(Screen screen2, int xa, int ya)
    {
      int[] pixels = screen2.Pixels;
      int index1 = 0;
      for (int index2 = 0; index2 < this.H; ++index2)
      {
        for (int index3 = 0; index3 < this.W; ++index3)
        {
          if (pixels[index1] / 10 <= this.dither[(index3 + xa & 3) + (index2 + ya & 3) * 4])
            this.Pixels[index1] = 0;
          ++index1;
        }
      }
    }

    public void RenderLight(int x, int y, int r)
    {
      x -= this.OffsetX;
      y -= this.OffsetY;
      int num1 = x - r;
      int num2 = x + r;
      int num3 = y - r;
      int num4 = y + r;
      if (num1 < 0)
        num1 = 0;
      if (num3 < 0)
        num3 = 0;
      if (num2 > this.W)
        num2 = this.W;
      if (num4 > this.H)
        num4 = this.H;
      for (int index1 = num3; index1 < num4; ++index1)
      {
        int num5 = index1 - y;
        int num6 = num5 * num5;
        for (int index2 = num1; index2 < num2; ++index2)
        {
          int num7 = index2 - x;
          int num8 = num7 * num7 + num6;
          if (num8 <= r * r)
          {
            int num9 = (int) byte.MaxValue - num8 * (int) byte.MaxValue / (r * r);
            if (this.Pixels[index2 + index1 * this.W] < num9)
              this.Pixels[index2 + index1 * this.W] = num9;
          }
        }
      }
    }
  }
}
