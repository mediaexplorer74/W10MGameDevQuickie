// Decompiled with JetBrains decompiler
// Type: GameEngine.EngineExtensions
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;

#nullable disable
namespace GameEngine
{
  public static class EngineExtensions
  {
    public static Vector2 ToVector2(this Point p) => new Vector2((float) p.X, (float) p.Y);

    public static bool Contains(this Rectangle rect, Vector2 point)
    {
      return rect.Contains(new Point((int) point.X, (int) point.Y));
    }

    public static Rectangle RebuildAroundZeroCenter(this Rectangle rect, Vector2 position)
    {
      return new Rectangle(-rect.Width / 2, -rect.Height / 2, rect.Width, rect.Height);
    }

    public static Rectangle TranslateRectangle(this Rectangle rect, Vector2 position)
    {
      return new Rectangle((int) position.X - rect.Width / 2, (int) position.Y - rect.Height / 2, rect.Width, rect.Height);
    }

    public static Vector2 CenterPivot(this Rectangle rect)
    {
      return new Vector2((float) (rect.Width / 2), (float) (rect.Height / 2));
    }

    public static Vector2 CenterPoint(this Rectangle rect)
    {
      return new Vector2((float) rect.Left + (float) (rect.Right - rect.Left) / 2f, (float) rect.Top + (float) (rect.Bottom - rect.Top) / 2f);
    }

    public static Vector2 ToInt(this Vector2 v)
    {
      return new Vector2((float) (int) v.X, (float) (int) v.Y);
    }

    public static Vector3 ToVector3(this Vector2 v) => new Vector3(v.X, v.Y, 0.0f);

    public static Vector2 ToVector2(this Vector3 v) => new Vector2(v.X, v.Y);

    public static Rectangle BuildRectangleAround(this Vector2 v, int width)
    {
      return new Rectangle((int) v.X - width / 2, (int) v.Y - width / 2, width, width);
    }

    public static Rectangle BuildRectangleAround(this Vector2 v, int width, int height)
    {
      return new Rectangle((int) v.X - width / 2, (int) v.Y - height / 2, width, height);
    }
  }
}
