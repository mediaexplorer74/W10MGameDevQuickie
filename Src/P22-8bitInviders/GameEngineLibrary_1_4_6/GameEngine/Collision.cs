// Decompiled with JetBrains decompiler
// Type: GameEngine.Collision
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System;

#nullable disable
namespace GameEngine
{
  public static class Collision
  {
    public static PolygonCollisionResult PolygonCollision(
      Polygon polygonA,
      Polygon polygonB,
      Vector2 velocity)
    {
      PolygonCollisionResult polygonCollisionResult = new PolygonCollisionResult();
      polygonCollisionResult.Intersect = true;
      polygonCollisionResult.WillIntersect = true;
      int count1 = polygonA.Edges.Count;
      int count2 = polygonB.Edges.Count;
      float num1 = float.PositiveInfinity;
      Vector2 vector2_1 = new Vector2();
      for (int index = 0; index < count1 + count2; ++index)
      {
        Vector2 vector2_2 = index >= count1 ? polygonB.Edges[index - count1] : polygonA.Edges[index];
        Vector2 axis = new Vector2(-vector2_2.Y, vector2_2.X);
        axis.Normalize();
        float min1 = 0.0f;
        float min2 = 0.0f;
        float max1 = 0.0f;
        float max2 = 0.0f;
        Collision.ProjectPolygon(axis, polygonA, ref min1, ref max1);
        Collision.ProjectPolygon(axis, polygonB, ref min2, ref max2);
        if ((double) Collision.IntervalDistance(min1, max1, min2, max2) > 0.0)
          polygonCollisionResult.Intersect = false;
        float num2 = Vector2.Dot(axis, velocity);
        if ((double) num2 < 0.0)
          min1 += num2;
        else
          max1 += num2;
        float num3 = Collision.IntervalDistance(min1, max1, min2, max2);
        if ((double) num3 > 0.0)
          polygonCollisionResult.WillIntersect = false;
        if (polygonCollisionResult.Intersect || polygonCollisionResult.WillIntersect)
        {
          float num4 = Math.Abs(num3);
          if ((double) num4 < (double) num1)
          {
            num1 = num4;
            vector2_1 = axis;
            if ((double) Vector2.Dot(polygonA.Center - polygonB.Center, vector2_1) < 0.0)
              vector2_1 = -vector2_1;
          }
        }
        else
          break;
      }
      if (polygonCollisionResult.WillIntersect)
        polygonCollisionResult.MinimumTranslationVector2 = vector2_1 * num1;
      return polygonCollisionResult;
    }

    public static float IntervalDistance(float minA, float maxA, float minB, float maxB)
    {
      return (double) minA < (double) minB ? minB - maxA : minA - maxB;
    }

    public static void ProjectPolygon(Vector2 axis, Polygon polygon, ref float min, ref float max)
    {
      float num1 = Vector2.Dot(axis, polygon.Points[0]);
      min = num1;
      max = num1;
      for (int index = 1; index < polygon.Points.Count; ++index)
      {
        float num2 = Vector2.Dot(polygon.Points[index], axis);
        if ((double) num2 < (double) min)
          min = num2;
        else if ((double) num2 > (double) max)
          max = num2;
      }
    }
  }
}
