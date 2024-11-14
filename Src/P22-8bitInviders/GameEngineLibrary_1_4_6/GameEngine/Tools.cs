// Decompiled with JetBrains decompiler
// Type: GameEngine.Tools
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#nullable disable
namespace GameEngine
{
  public static class Tools
  {
    private static TimeSpan timeStart;
    private static char[] numberBuffer = new char[16];

    public static void Trace(string x)
    {
    }

    public static StringBuilder AppendNumber(this StringBuilder stringBuilder, int number)
    {
      return stringBuilder.AppendNumber(number, 0);
    }

    public static StringBuilder AppendNumber(
      this StringBuilder stringBuilder,
      int number,
      int minDigits)
    {
      if (number < 0)
      {
        stringBuilder.Append('-');
        number = -number;
      }
      int index1 = 0;
      do
      {
        int num = number % 10;
        Tools.numberBuffer[index1] = (char) (48 + num);
        number /= 10;
        ++index1;
      }
      while (number > 0 || index1 < minDigits);
      for (int index2 = index1 - 1; index2 >= 0; --index2)
        stringBuilder.Append(Tools.numberBuffer[index2]);
      return stringBuilder;
    }

    public static int[] GenerateRandomNumbers(int max)
    {
      if (max == 0)
        return new int[1]{ 0 };
      int[] randomNumbers = new int[max];
      List<int> intList = new List<int>();
      for (int index = 0; index < max; ++index)
        intList.Add(index);
      for (int index1 = 0; index1 < max; ++index1)
      {
        int index2 = Engine.random.Next(intList.Count);
        randomNumbers[index1] = intList[index2];
        intList.RemoveAt(index2);
      }
      return randomNumbers;
    }

    public static Rectangle ComputeRect(Vector2 position, Texture2D tex)
    {
      return new Rectangle((int) ((double) position.X - (double) tex.Width / 2.0), (int) ((double) position.Y - (double) tex.Height / 2.0), tex.Width, tex.Height);
    }

    public static Rectangle ComputeRect(Vector2 position, int width, int height)
    {
      return new Rectangle((int) ((double) position.X - (double) width / 2.0), (int) ((double) position.Y - (double) height / 2.0), width, height);
    }

    public static string GetAppVersion()
    {
       return "v1.0";//Assembly.GetCallingAssembly().FullName.Split(',')[1].Split('=')[1].Substring(0, 3);
    }

    public static Texture2D TintColor(Texture2D tex, Color tint, int strength, MixMethod method)
    {
      strength = (int) MathHelper.Clamp((float) strength, 0.0f, 100f);
      if (method == MixMethod.ColorTint)
        strength = (int) MathHelper.Lerp(0.0f, (float) byte.MaxValue, (float) strength / 100f);
      byte num1 = (byte) (100 - strength);
      Color[] data = new Color[tex.Width * tex.Height];
      Texture2D texture2D = new Texture2D(Engine.gdm.GraphicsDevice, tex.Width, tex.Height);
      tex.GetData<Color>(data);
      for (int index = 0; index < tex.Width * tex.Height; ++index)
      {
        byte r;
        byte g;
        byte b;
        if (method == MixMethod.ColorTint)
        {
          double num2 = (double) ((int) data[index].R * 30 + (int) data[index].G * 59 + (int) data[index].B * 11) / 100.0 / (double) byte.MaxValue;
          float num3 = (float) tint.R / (float) byte.MaxValue;
          r = Convert.ToByte(MathHelper.Clamp((float) ((double) tint.R * num2 + (double) num3 * (double) strength), 0.0f, (float) byte.MaxValue));
          float num4 = (float) tint.G / (float) byte.MaxValue;
          g = Convert.ToByte(MathHelper.Clamp((float) ((double) tint.G * num2 + (double) num4 * (double) strength), 0.0f, (float) byte.MaxValue));
          float num5 = (float) tint.B / (float) byte.MaxValue;
          b = Convert.ToByte(MathHelper.Clamp((float) ((double) tint.B * num2 + (double) num5 * (double) strength), 0.0f, (float) byte.MaxValue));
        }
        else
        {
          r = Convert.ToByte(((int) num1 * (int) data[index].R + strength * (int) tint.R) / 100);
          g = Convert.ToByte(((int) num1 * (int) data[index].G + strength * (int) tint.G) / 100);
          b = Convert.ToByte(((int) num1 * (int) data[index].B + strength * (int) tint.B) / 100);
        }
        data[index] = Color.FromNonPremultiplied((int) r, (int) g, (int) b, (int) data[index].A);
      }
      texture2D.SetData<Color>(data);
      return texture2D;
    }

    public static float SineAnimation(GameTime gameTime, float low, float high)
    {
      float num = (float) Math.Sin(gameTime.TotalGameTime.TotalSeconds);
      return low + (float) (((double) num - -1.0) * ((double) high - (double) low) / 2.0);
    }

    public static float SineAnimation(
      GameTime gameTime,
      float frequencyMultiplier,
      float low,
      float high)
    {
      float num = (float) Math.Sin(gameTime.TotalGameTime.TotalSeconds * (double) frequencyMultiplier);
      return low + (float) (((double) num - -1.0) * ((double) high - (double) low) / 2.0);
    }

    public static float SineAnimation(float low, float high, float sinGT)
    {
      float num = sinGT;
      return low + (float) (((double) num - -1.0) * ((double) high - (double) low) / 2.0);
    }

    public static float RemapValue(float value, float low1, float high1, float low2, float high2)
    {
      value = MathHelper.Clamp(value, low1, high1);
      return (double) low2 < (double) high2 ? low2 + (float) (((double) value - (double) low1) * ((double) high2 - (double) low2) / ((double) high1 - (double) low1)) : high2 + (float) (((double) value - (double) high1) * ((double) low2 - (double) high2) / ((double) low1 - (double) high1));
    }

    public static float WrapValue(float value, float min, float max)
    {
      if ((double) value > (double) max)
        return value - max + min;
      return (double) value < (double) min ? max - (min - value) : value;
    }

    public static int RandomNumber() => Engine.random.Next();

    public static float RandomFloat(float min, float max)
    {
      return min + (float) Engine.random.NextDouble() * (max - min);
    }

    public static int RandomInt(int min, int max) => Engine.random.Next(min, max);

    public static int RandomInt(int max) => Engine.random.Next(max);

    public static Vector3 RandomV3(float min, float max)
    {
      return new Vector3(Tools.RandomFloat(min, max), Tools.RandomFloat(min, max), Tools.RandomFloat(min, max));
    }

    public static Vector3 RandomV3(Vector3 min, Vector3 max)
    {
      return new Vector3()
      {
        X = Tools.RandomFloat(min.X, max.X),
        Y = Tools.RandomFloat(min.Y, max.Y),
        Z = Tools.RandomFloat(min.Z, max.Z)
      };
    }

    public static Color RandomColor()
    {
      return new Color(new Vector3(Tools.RandomFloat(0.0f, 1f), Tools.RandomFloat(0.0f, 1f), Tools.RandomFloat(0.0f, 1f)));
    }

    public static bool RandomBool() => Tools.RandomInt(100) < 50;

    public static T GetScreenByName<T>(string name) where T : GameScreen
    {
      return Engine.screenManager.GetScreenByName<T>(name);
    }

    public static T GetScreenByType<T>() where T : GameScreen
    {
      return Engine.screenManager.GetScreenByType<T>();
    }

    public static float AngleBetweenPositions(Vector2 p1, Vector2 p2)
    {
      return (float) Math.Atan2((double) p2.Y - (double) p1.Y, (double) p2.X - (double) p1.X);
    }

    public static Vector2 AngleToVector(float angle)
    {
      return new Vector2((float) Math.Sin((double) angle), -(float) Math.Cos((double) angle));
    }

    public static float VectorToAngle(Vector2 vector)
    {
      return (float) Math.Atan2((double) vector.X, -(double) vector.Y);
    }

    public static float Bounce(float scalar, float maxValue)
    {
      return (float) Math.Sin((double) Tools.RemapValue(scalar, 0.0f, 1f, 0.0f, 3.14159274f)) * maxValue;
    }

    public static void StartMemoryMonitor()
    {
    }

    public static int[] NumberArrayFromInt(int num)
    {
      Stack<int> intStack = new Stack<int>();
      do
      {
        intStack.Push(num % 10);
        num /= 10;
      }
      while (num > 0);
      int[] numArray = new int[intStack.Count];
      int count = intStack.Count;
      for (int index = 0; index < count; ++index)
        numArray[index] = intStack.Pop();
      return numArray;
    }

    public static bool WithinToleranceV2(float value1, float value2, float tolerance)
    {
      if ((double) value1 >= 0.0 && (double) value2 >= 0.0)
        return (double) Math.Abs(value1 - value2) <= (double) Math.Abs(tolerance);
      if ((double) value1 < 0.0 && (double) value2 < 0.0)
        return (double) Math.Abs(Math.Abs(value1) + value2) <= (double) Math.Abs(tolerance);
      return (double) value1 < 0.0 && (double) value2 >= 0.0 || (double) value1 >= 0.0 && (double) value2 < 0.0 ? (double) Math.Abs(value1 + value2) <= (double) Math.Abs(tolerance) : (double) Math.Abs(value1 - value2) <= (double) Math.Abs(tolerance);
    }

    public static bool WithinTolerance(float value1, float value2, float tolerance)
    {
      return (double) Math.Abs(value1 - value2) <= (double) Math.Abs(tolerance);
    }

    public static void StopwatchStart() => Tools.timeStart = TimeSpan.FromTicks(DateTime.Now.Ticks);

    public static TimeSpan StopwatchStop()
    {
      return TimeSpan.FromTicks(DateTime.Now.Ticks) - Tools.timeStart;
    }

    public static void AssignSourceRects(SceneNode searchNode)
    {
      foreach (SceneNode sceneNode in (List<SceneNode>) searchNode)
        sceneNode.SetSourceRect();
    }

    public static float CalculateSpeed(Vector2 p1, Vector2 p2, TimeSpan time)
    {
      return Vector2.Distance(p1, p2) / (float) time.TotalSeconds;
    }

    public static float CalculateDuration(this Vector2 p1, Vector2 p2, float speed)
    {
      return (float) TimeSpan.FromSeconds((double) Vector2.Distance(p1, p2) / (double) speed).TotalSeconds;
    }

    public static float GTElapsed(GameTime gameTime)
    {
      return (float) gameTime.ElapsedGameTime.TotalSeconds;
    }

    public static Vector2 ConvertTo2DCoords(Vector3 coords) => new Vector2(-coords.Z, coords.X);

    public static Vector3 ConvertTo3DCoords(Vector2 coords)
    {
      return new Vector3(coords.Y, 0.0f, -coords.X);
    }
  }
}
