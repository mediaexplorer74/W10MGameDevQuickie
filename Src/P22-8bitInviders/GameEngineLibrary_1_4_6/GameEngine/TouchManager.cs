// Decompiled with JetBrains decompiler
// Type: GameEngine.TouchManager
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public static class TouchManager
  {
    private static List<int> touchIDList = new List<int>();

    public static bool AddID(int id)
    {
      if (TouchManager.touchIDList.Contains(id))
        return false;
      TouchManager.touchIDList.Add(id);
      return true;
    }

    public static void RemoveID(int id)
    {
      if (!TouchManager.touchIDList.Contains(id))
        return;
      TouchManager.touchIDList.Remove(id);
    }

    public static bool IsTracking(int id) => TouchManager.touchIDList.Contains(id);
  }
}
