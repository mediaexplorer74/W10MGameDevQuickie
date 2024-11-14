// Decompiled with JetBrains decompiler
// Type: GameEngine.Polygon
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class Polygon
  {
    private List<Vector2> points = new List<Vector2>();
    private List<Vector2> edges = new List<Vector2>();
    private List<Vector2> originalPoints = new List<Vector2>();

    public void AddPoint(Vector2 where)
    {
      this.points.Add(where);
      this.originalPoints.Add(where);
    }

    public void FinalizePolygon()
    {
      this.edges.Clear();
      for (int index = 0; index < this.points.Count; ++index)
      {
        Vector2 point = this.points[index];
        this.edges.Add((index + 1 < this.points.Count ? this.points[index + 1] : this.points[0]) - point);
      }
    }

    public List<Vector2> Edges => this.edges;

    public List<Vector2> Points => this.points;

    public Vector2 Center
    {
      get
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        for (int index = 0; index < this.points.Count; ++index)
        {
          num1 += this.points[index].X;
          num2 += this.points[index].Y;
        }
        return new Vector2(num1 / (float) this.points.Count, num2 / (float) this.points.Count);
      }
    }

    public void ModifyPoint(int pointIndex, Vector2 where)
    {
      if (pointIndex >= this.points.Count)
        return;
      this.points[pointIndex] = where;
      this.originalPoints[pointIndex] = where;
    }

    public void Transform(Vector2 where, float rotation)
    {
      for (int index = 0; index < this.originalPoints.Count; ++index)
      {
        Matrix translation1 = Matrix.CreateTranslation(where.ToVector3());
        Matrix translation2 = Matrix.CreateTranslation(this.originalPoints[index].ToVector3());
        Matrix rotationZ = Matrix.CreateRotationZ(rotation);
        Matrix identity = Matrix.Identity;
        Matrix matrix = Matrix.Identity * translation2 * rotationZ * translation1;
        this.Points[index] = matrix.Translation.ToVector2();
      }
    }

    public override string ToString()
    {
      string str = "";
      for (int index = 0; index < this.points.Count; ++index)
      {
        if (str != "")
          str += " ";
        str = str + "{" + this.points[index].ToString() + "}";
      }
      return str;
    }

    public void Draw()
    {
      for (int index = 0; index < this.Points.Count - 1; ++index)
        Lines.DrawLine(this.Points[index], this.Points[index + 1]);
      Lines.DrawLine(this.Points[this.Points.Count - 1], this.Points[0]);
    }
  }
}
