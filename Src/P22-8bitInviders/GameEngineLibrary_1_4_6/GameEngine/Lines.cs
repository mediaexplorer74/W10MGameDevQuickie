// Decompiled with JetBrains decompiler
// Type: GameEngine.Lines
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public static class Lines
  {
    private static Texture2D tex;
    private static List<Vector2> startList = new List<Vector2>();
    private static List<Vector2> endList = new List<Vector2>();
    private static List<Color> colorList = new List<Color>();
    private static bool set_data = false;

    public static void DrawLine(Vector2 start, Vector2 end)
    {
      Lines.startList.Add(start);
      Lines.endList.Add(end);
      Lines.colorList.Add(Color.White);
    }

    public static void DrawLine(Vector2 start, Vector2 end, Color color)
    {
      Lines.startList.Add(start);
      Lines.endList.Add(end);
      Lines.colorList.Add(color);
    }

    public static void DrawRectangle(Rectangle rect)
    {
      Lines.startList.Add(new Vector2((float) rect.Left, (float) rect.Top));
      Lines.endList.Add(new Vector2((float) rect.Right, (float) rect.Top));
      Lines.colorList.Add(Color.White);
      Lines.startList.Add(new Vector2((float) rect.Left, (float) rect.Top));
      Lines.endList.Add(new Vector2((float) rect.Left, (float) rect.Bottom));
      Lines.colorList.Add(Color.White);
      Lines.startList.Add(new Vector2((float) rect.Right, (float) rect.Top));
      Lines.endList.Add(new Vector2((float) rect.Right, (float) rect.Bottom));
      Lines.colorList.Add(Color.White);
      Lines.startList.Add(new Vector2((float) rect.Left, (float) rect.Bottom));
      Lines.endList.Add(new Vector2((float) rect.Right, (float) rect.Bottom));
      Lines.colorList.Add(Color.White);
    }

    public static void Draw(SpriteBatch sb) => Lines.Draw(sb, true);

    public static void Draw(SpriteBatch sb, bool beginBatch)
    {
      if (!Lines.set_data)
      {
        Lines.tex = new Texture2D(Engine.gdm.GraphicsDevice, 1, 1);
        Lines.tex.SetData<Color>(new Color[1]{ Color.White });
        Lines.set_data = true;
      }
      if (beginBatch)
        sb.Begin();
      for (int index = 0; index < Lines.startList.Count; ++index)
      {
        float rotation = (float) Math.Atan2((double) Lines.endList[index].Y - (double) Lines.startList[index].Y, (double) Lines.endList[index].X - (double) Lines.startList[index].X);
        float x = (Lines.endList[index] - Lines.startList[index]).Length();
        sb.Draw(Lines.tex, Lines.startList[index], new Rectangle?(), Lines.colorList[index], rotation, Vector2.Zero, new Vector2(x, 1f), SpriteEffects.None, 0.0f);
      }
      if (beginBatch)
        sb.End();
      Lines.startList.Clear();
      Lines.endList.Clear();
      Lines.colorList.Clear();
    }
  }
}
