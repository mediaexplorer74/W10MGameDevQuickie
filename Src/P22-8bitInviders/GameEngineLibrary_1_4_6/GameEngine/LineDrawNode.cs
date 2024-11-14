// Decompiled with JetBrains decompiler
// Type: GameEngine.LineDrawNode
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class LineDrawNode : SceneNode
  {
    private PrimitiveBatch primLines;
    private List<WaypointList> wpList;
    private int wpcount;
    private int maxwp;
    private Color color;

    public LineDrawNode(Color lineColor, int maxpoints, GameScreen screen)
      : base(nameof (LineDrawNode), screen)
    {
      this.maxwp = maxpoints;
      this.color = lineColor;
      this.Initialize();
    }

    public override void Initialize()
    {
      this.primLines = new PrimitiveBatch();
      this.wpList = new List<WaypointList>(10);
    }

    public void AddPathToDraw(WaypointList wplist)
    {
      if (wplist.Count <= 0 || this.wpcount + 1 > this.maxwp)
        return;
      ++this.wpcount;
      this.wpList.Add(wplist);
    }

    public override void Draw()
    {
      if (this.wpList.Count <= 0)
        return;
      this.primLines.Begin(PrimType.LineList);
      foreach (WaypointList wp in this.wpList)
      {
        if (wp.Count >= 2)
        {
          this.primLines.AddVertex(wp[0], this.color);
          for (int index = 1; index < wp.Count; ++index)
          {
            this.primLines.AddVertex(wp[index], this.color);
            if (index < wp.Count - 1)
              this.primLines.AddVertex(wp[index], this.color);
          }
        }
      }
      this.primLines.End();
      this.wpList.Clear();
      this.wpcount = 0;
    }
  }
}
