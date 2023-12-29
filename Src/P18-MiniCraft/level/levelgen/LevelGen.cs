// Decompiled with JetBrains decompiler
// Type: GameManager.level.levelgen.LevelGen
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.level.tile;
using System;

#nullable disable
namespace GameManager.level.levelgen
{
  public class LevelGen
  {
    public double[] Values;
    private static readonly Random random = new Random();
    private int w;
    private int h;

    public LevelGen(int w, int h, int featureSize)
    {
      this.w = w;
      this.h = h;
      this.Values = new double[w * h];
      for (int y = 0; y < w; y += featureSize)
      {
        for (int x = 0; x < w; x += featureSize)
          this.setSample(x, y, LevelGen.random.NextDouble() * 2.0 - 1.0);
      }
      int num1 = featureSize;
      double num2 = 1.0 / (double) w;
      double num3 = 1.0;
      do
      {
        int num4 = num1 / 2;
        for (int y = 0; y < w; y += num1)
        {
          for (int x = 0; x < w; x += num1)
          {
            double num5 = (this.sample(x, y) + this.sample(x + num1, y) + this.sample(x, y + num1) + this.sample(x + num1, y + num1)) / 4.0 + (LevelGen.random.NextDouble() * 2.0 - 1.0) * (double) num1 * num2;
            this.setSample(x + num4, y + num4, num5);
          }
        }
        for (int y = 0; y < w; y += num1)
        {
          for (int x = 0; x < w; x += num1)
          {
            double num6 = this.sample(x, y);
            double num7 = this.sample(x + num1, y);
            double num8 = this.sample(x, y + num1);
            double num9 = this.sample(x + num4, y + num4);
            double num10 = this.sample(x + num4, y - num4);
            double num11 = this.sample(x - num4, y + num4);
            double num12 = (num6 + num7 + num9 + num10) / 4.0 + (LevelGen.random.NextDouble() * 2.0 - 1.0) * (double) num1 * num2 * 0.5;
            double num13 = (num6 + num8 + num9 + num11) / 4.0 + (LevelGen.random.NextDouble() * 2.0 - 1.0) * (double) num1 * num2 * 0.5;
            this.setSample(x + num4, y, num12);
            this.setSample(x, y + num4, num13);
          }
        }
        num1 /= 2;
        num2 *= num3 + 0.8;
        num3 *= 0.3;
      }
      while (num1 > 1);
    }

    public static byte[][] CreateAndValidateTopMap(int w, int h)
    {
      byte[][] topMap;
      int[] numArray;
      do
      {
        topMap = LevelGen.createTopMap(w, h);
        numArray = new int[256];
        for (int index = 0; index < w * h; ++index)
          ++numArray[(int) topMap[0][index] & (int) byte.MaxValue];
      }
      while (numArray[(int) Tile.Rock.Id & (int) byte.MaxValue] < 100 || numArray[(int) Tile.Sand.Id & (int) byte.MaxValue] < 100 || numArray[(int) Tile.Grass.Id & (int) byte.MaxValue] < 100 || numArray[(int) Tile.Tree.Id & (int) byte.MaxValue] < 100 || numArray[(int) Tile.StairsDown.Id & (int) byte.MaxValue] < 2);
      return topMap;
    }

    public static byte[][] CreateAndValidateUndergroundMap(int w, int h, int depth)
    {
      byte[][] undergroundMap;
      int[] numArray;
      do
      {
        undergroundMap = LevelGen.createUndergroundMap(w, h, depth);
        numArray = new int[256];
        for (int index = 0; index < w * h; ++index)
          ++numArray[(int) undergroundMap[0][index] & (int) byte.MaxValue];
      }
      while (numArray[(int) Tile.Rock.Id & (int) byte.MaxValue] < 100 || numArray[(int) Tile.Dirt.Id & (int) byte.MaxValue] < 100 || numArray[((int) Tile.IronOre.Id & (int) byte.MaxValue) + depth - 1] < 20 || depth < 3 && numArray[(int) Tile.StairsDown.Id & (int) byte.MaxValue] < 2);
      return undergroundMap;
    }

    public static byte[][] CreateAndValidateSkyMap(int w, int h)
    {
      byte[][] skyMap;
      int[] numArray;
      do
      {
        skyMap = LevelGen.createSkyMap(w, h);
        numArray = new int[256];
        for (int index = 0; index < w * h; ++index)
          ++numArray[(int) skyMap[0][index] & (int) byte.MaxValue];
      }
      while (numArray[(int) Tile.Cloud.Id & (int) byte.MaxValue] < 2000 || numArray[(int) Tile.StairsDown.Id & (int) byte.MaxValue] < 2);
      return skyMap;
    }

    private static byte[][] createTopMap(int w, int h)
    {
      LevelGen levelGen1 = new LevelGen(w, h, 16);
      LevelGen levelGen2 = new LevelGen(w, h, 16);
      LevelGen levelGen3 = new LevelGen(w, h, 16);
      LevelGen levelGen4 = new LevelGen(w, h, 32);
      LevelGen levelGen5 = new LevelGen(w, h, 32);
      byte[] numArray1 = new byte[w * h];
      byte[] numArray2 = new byte[w * h];
      for (int index1 = 0; index1 < h; ++index1)
      {
        for (int index2 = 0; index2 < w; ++index2)
        {
          int index3 = index2 + index1 * w;
          double num1 = Math.Abs(levelGen4.Values[index3] - levelGen5.Values[index3] * 3.0) - 2.0;
          double num2 = Math.Abs(Math.Abs(levelGen1.Values[index3] - levelGen2.Values[index3]) - levelGen3.Values[index3] * 3.0) - 2.0;
          double num3 = (double) index2 / ((double) w - 1.0) * 2.0 - 1.0;
          double num4 = (double) index1 / ((double) h - 1.0) * 2.0 - 1.0;
          if (num3 < 0.0)
            num3 = -num3;
          if (num4 < 0.0)
            num4 = -num4;
          double num5 = num3 >= num4 ? num3 : num4;
          double num6 = num5 * num5 * num5 * num5;
          double num7 = num6 * num6 * num6 * num6;
          double num8 = num1 + 1.0 - num7 * 20.0;
          numArray1[index3] = num8 >= -0.5 ? (num8 <= 0.5 || num2 >= -1.5 ? Tile.Grass.Id : Tile.Rock.Id) : Tile.Water.Id;
        }
      }
      for (int index4 = 0; index4 < w * h / 2800; ++index4)
      {
        int num9 = LevelGen.random.Next(w);
        int num10 = LevelGen.random.Next(h);
        for (int index5 = 0; index5 < 10; ++index5)
        {
          int num11 = num9 + LevelGen.random.Next(21) - 10;
          int num12 = num10 + LevelGen.random.Next(21) - 10;
          for (int index6 = 0; index6 < 100; ++index6)
          {
            int num13 = num11 + LevelGen.random.Next(5) - LevelGen.random.Next(5);
            int num14 = num12 + LevelGen.random.Next(5) - LevelGen.random.Next(5);
            for (int index7 = num14 - 1; index7 <= num14 + 1; ++index7)
            {
              for (int index8 = num13 - 1; index8 <= num13 + 1; ++index8)
              {
                if (index8 >= 0 && index7 >= 0 && index8 < w && index7 < h && (int) numArray1[index8 + index7 * w] == (int) Tile.Grass.Id)
                  numArray1[index8 + index7 * w] = Tile.Sand.Id;
              }
            }
          }
        }
      }
      for (int index9 = 0; index9 < w * h / 400; ++index9)
      {
        int num15 = LevelGen.random.Next(w);
        int num16 = LevelGen.random.Next(h);
        for (int index10 = 0; index10 < 200; ++index10)
        {
          int num17 = num15 + LevelGen.random.Next(15) - LevelGen.random.Next(15);
          int num18 = num16 + LevelGen.random.Next(15) - LevelGen.random.Next(15);
          if (num17 >= 0 && num18 >= 0 && num17 < w && num18 < h && (int) numArray1[num17 + num18 * w] == (int) Tile.Grass.Id)
            numArray1[num17 + num18 * w] = Tile.Tree.Id;
        }
      }
      for (int index11 = 0; index11 < w * h / 400; ++index11)
      {
        int num19 = LevelGen.random.Next(w);
        int num20 = LevelGen.random.Next(h);
        int num21 = LevelGen.random.Next(4);
        for (int index12 = 0; index12 < 30; ++index12)
        {
          int num22 = num19 + LevelGen.random.Next(5) - LevelGen.random.Next(5);
          int num23 = num20 + LevelGen.random.Next(5) - LevelGen.random.Next(5);
          if (num22 >= 0 && num23 >= 0 && num22 < w && num23 < h && (int) numArray1[num22 + num23 * w] == (int) Tile.Grass.Id)
          {
            numArray1[num22 + num23 * w] = Tile.Flower.Id;
            numArray2[num22 + num23 * w] = (byte) (num21 + LevelGen.random.Next(4) * 16);
          }
        }
      }
      for (int index = 0; index < w * h / 100; ++index)
      {
        int num24 = LevelGen.random.Next(w);
        int num25 = LevelGen.random.Next(h);
        if (num24 >= 0 && num25 >= 0 && num24 < w && num25 < h && (int) numArray1[num24 + num25 * w] == (int) Tile.Sand.Id)
          numArray1[num24 + num25 * w] = Tile.Cactus.Id;
      }
      int num26 = 0;
      for (int index13 = 0; index13 < w * h / 100; ++index13)
      {
        bool flag = false;
        int num27 = LevelGen.random.Next(w - 2) + 1;
        int num28 = LevelGen.random.Next(h - 2) + 1;
        for (int index14 = num28 - 1; index14 <= num28 + 1; ++index14)
        {
          for (int index15 = num27 - 1; index15 <= num27 + 1; ++index15)
          {
            if ((int) numArray1[index15 + index14 * w] != (int) Tile.Rock.Id)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            break;
        }
        if (!flag)
        {
          numArray1[num27 + num28 * w] = Tile.StairsDown.Id;
          ++num26;
          if (num26 == 4)
            break;
        }
      }
      return new byte[2][]{ numArray1, numArray2 };
    }

    private static byte[][] createUndergroundMap(int w, int h, int depth)
    {
      LevelGen levelGen1 = new LevelGen(w, h, 16);
      LevelGen levelGen2 = new LevelGen(w, h, 16);
      LevelGen levelGen3 = new LevelGen(w, h, 16);
      LevelGen levelGen4 = new LevelGen(w, h, 16);
      LevelGen levelGen5 = new LevelGen(w, h, 16);
      LevelGen levelGen6 = new LevelGen(w, h, 16);
      LevelGen levelGen7 = new LevelGen(w, h, 16);
      LevelGen levelGen8 = new LevelGen(w, h, 16);
      LevelGen levelGen9 = new LevelGen(w, h, 16);
      LevelGen levelGen10 = new LevelGen(w, h, 32);
      LevelGen levelGen11 = new LevelGen(w, h, 32);
      byte[] numArray1 = new byte[w * h];
      byte[] numArray2 = new byte[w * h];
      for (int index1 = 0; index1 < h; ++index1)
      {
        for (int index2 = 0; index2 < w; ++index2)
        {
          int index3 = index2 + index1 * w;
          double num1 = Math.Abs(levelGen10.Values[index3] - levelGen11.Values[index3]) * 3.0 - 2.0;
          double num2 = Math.Abs(Math.Abs(levelGen1.Values[index3] - levelGen2.Values[index3]) - levelGen3.Values[index3]) * 3.0 - 2.0;
          double num3 = Math.Abs(Math.Abs(levelGen4.Values[index3] - levelGen5.Values[index3]) - levelGen6.Values[index3]) * 3.0 - 2.0;
          Math.Abs(levelGen7.Values[index3] - levelGen8.Values[index3]);
          double num4 = Math.Abs(num3 - levelGen9.Values[index3]) * 3.0 - 2.0;
          double num5 = (double) index2 / ((double) w - 1.0) * 2.0 - 1.0;
          double num6 = (double) index1 / ((double) h - 1.0) * 2.0 - 1.0;
          if (num5 < 0.0)
            num5 = -num5;
          if (num6 < 0.0)
            num6 = -num6;
          double num7 = num5 >= num6 ? num5 : num6;
          double num8 = num7 * num7 * num7 * num7;
          double num9 = num8 * num8 * num8 * num8;
          double num10 = num1 + 1.0 - num9 * 20.0;
          numArray1[index3] = num10 <= -2.0 || num4 >= (double) (depth / 2 * 3) - 2.0 ? (num10 <= -2.0 || num2 >= -1.7 && num3 >= -1.4 ? Tile.Rock.Id : Tile.Dirt.Id) : (depth <= 2 ? Tile.Water.Id : Tile.Lava.Id);
        }
      }
      int num11 = 2;
      for (int index4 = 0; index4 < w * h / 400; ++index4)
      {
        int num12 = LevelGen.random.Next(w);
        int num13 = LevelGen.random.Next(h);
        for (int index5 = 0; index5 < 30; ++index5)
        {
          int num14 = num12 + LevelGen.random.Next(5) - LevelGen.random.Next(5);
          int num15 = num13 + LevelGen.random.Next(5) - LevelGen.random.Next(5);
          if (num14 >= num11 && num15 >= num11 && num14 < w - num11 && num15 < h - num11 && (int) numArray1[num14 + num15 * w] == (int) Tile.Rock.Id)
            numArray1[num14 + num15 * w] = (byte) (((int) Tile.IronOre.Id & (int) byte.MaxValue) + depth - 1);
        }
      }
      if (depth < 3)
      {
        int num16 = 0;
        for (int index6 = 0; index6 < w * h / 100; ++index6)
        {
          bool flag = false;
          int num17 = LevelGen.random.Next(w - 20) + 10;
          int num18 = LevelGen.random.Next(h - 20) + 10;
          for (int index7 = num18 - 1; index7 <= num18 + 1; ++index7)
          {
            for (int index8 = num17 - 1; index8 <= num17 + 1; ++index8)
            {
              if ((int) numArray1[index8 + index7 * w] != (int) Tile.Rock.Id)
              {
                flag = true;
                break;
              }
            }
            if (flag)
              break;
          }
          if (!flag)
          {
            numArray1[num17 + num18 * w] = Tile.StairsDown.Id;
            ++num16;
            if (num16 == 4)
              break;
          }
        }
      }
      return new byte[2][]{ numArray1, numArray2 };
    }

    private static byte[][] createSkyMap(int w, int h)
    {
      LevelGen levelGen1 = new LevelGen(w, h, 8);
      LevelGen levelGen2 = new LevelGen(w, h, 8);
      byte[] numArray1 = new byte[w * h];
      byte[] numArray2 = new byte[w * h];
      for (int index1 = 0; index1 < h; ++index1)
      {
        for (int index2 = 0; index2 < w; ++index2)
        {
          int index3 = index2 + index1 * w;
          double num1 = Math.Abs(levelGen1.Values[index3] - levelGen2.Values[index3]) * 3.0 - 2.0;
          double num2 = (double) index2 / ((double) w - 1.0) * 2.0 - 1.0;
          double num3 = (double) index1 / ((double) h - 1.0) * 2.0 - 1.0;
          if (num2 < 0.0)
            num2 = -num2;
          if (num3 < 0.0)
            num3 = -num3;
          double num4 = num2 >= num3 ? num2 : num3;
          double num5 = num4 * num4 * num4 * num4;
          double num6 = num5 * num5 * num5 * num5;
          numArray1[index3] = -num1 * 1.0 - 2.2 + 1.0 - num6 * 20.0 >= -0.25 ? Tile.Cloud.Id : Tile.InfiniteFall.Id;
        }
      }
      for (int index4 = 0; index4 < w * h / 50; ++index4)
      {
        bool flag = false;
        int num7 = LevelGen.random.Next(w - 2) + 1;
        int num8 = LevelGen.random.Next(h - 2) + 1;
        for (int index5 = num8 - 1; index5 <= num8 + 1; ++index5)
        {
          for (int index6 = num7 - 1; index6 <= num7 + 1; ++index6)
          {
            if ((int) numArray1[index6 + index5 * w] != (int) Tile.Cloud.Id)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            break;
        }
        if (!flag)
          numArray1[num7 + num8 * w] = Tile.CloudCactus.Id;
      }
      int num9 = 0;
      for (int index7 = 0; index7 < w * h; ++index7)
      {
        bool flag = false;
        int num10 = LevelGen.random.Next(w - 2) + 1;
        int num11 = LevelGen.random.Next(h - 2) + 1;
        for (int index8 = num11 - 1; index8 <= num11 + 1; ++index8)
        {
          for (int index9 = num10 - 1; index9 <= num10 + 1; ++index9)
          {
            if ((int) numArray1[index9 + index8 * w] != (int) Tile.Cloud.Id)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            break;
        }
        if (!flag)
        {
          numArray1[num10 + num11 * w] = Tile.StairsDown.Id;
          ++num9;
          if (num9 == 2)
            break;
        }
      }
      return new byte[2][]{ numArray1, numArray2 };
    }

    private double sample(int x, int y)
    {
      return this.Values[(x & this.w - 1) + (y & this.h - 1) * this.w];
    }

    private void setSample(int x, int y, double value)
    {
      this.Values[(x & this.w - 1) + (y & this.h - 1) * this.w] = value;
    }
  }
}
