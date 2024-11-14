// Decompiled with JetBrains decompiler
// Type: GameEngine.WaypointList3D
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class WaypointList3D : Queue<Vector3>
  {
    public Vector3 this[int index]
    {
      get
      {
        Vector3 vector3_1 = Vector3.Zero;
        foreach (Vector3 vector3_2 in (Queue<Vector3>) this)
        {
          --index;
          if (index < 0)
          {
            vector3_1 = vector3_2;
            break;
          }
        }
        return vector3_1;
      }
    }
  }
}
