// Decompiled with JetBrains decompiler
// Type: GameEngine.WaypointList
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class WaypointList : Queue<Vector2>
  {
    public Vector2 this[int index]
    {
      get
      {
        Vector2 vector2_1 = Vector2.Zero;
        foreach (Vector2 vector2_2 in (Queue<Vector2>) this)
        {
          --index;
          if (index < 0)
          {
            vector2_1 = vector2_2;
            break;
          }
        }
        return vector2_1;
      }
    }
  }
}
