// Decompiled with JetBrains decompiler
// Type: GameEngine.AdvancedObjectPool`1
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class AdvancedObjectPool<T> : IEnumerable<T>, IEnumerable
  {
    private AdvancedObjectPool<T>.Node[] Pool;
    private bool[] isActive;
    private Queue<int> Available;
    private int currentIndex;
    private int maxObjectCount;

    public int AvailableCount => this.Available.Count;

    public int ActiveCount => this.Pool.Length - this.Available.Count;

    public int Capacity => this.Pool.Length;

    public AdvancedObjectPool(int objectCount)
    {
      this.maxObjectCount = objectCount;
      this.currentIndex = 0;
      this.Pool = new AdvancedObjectPool<T>.Node[objectCount];
      this.isActive = new bool[objectCount];
      this.Available = new Queue<int>(objectCount);
    }

    public void AddToPool(T tile)
    {
      if (this.currentIndex >= this.maxObjectCount)
        return;
      this.Pool[this.currentIndex] = new AdvancedObjectPool<T>.Node();
      this.Pool[this.currentIndex].NodeIndex = this.currentIndex;
      this.Pool[this.currentIndex].Item = tile;
      this.isActive[this.currentIndex] = false;
      this.Available.Enqueue(this.currentIndex);
      ++this.currentIndex;
    }

    public void Clear()
    {
      this.Available.Clear();
      for (int index = 0; index < this.Pool.Length; ++index)
      {
        this.isActive[index] = false;
        this.Available.Enqueue(index);
      }
    }

    public AdvancedObjectPool<T>.Node Get()
    {
      int index = this.Available.Dequeue();
      this.isActive[index] = true;
      return this.Pool[index];
    }

    public void Return(AdvancedObjectPool<T>.Node item)
    {
      if (item.NodeIndex < 0 || item.NodeIndex > this.Pool.Length)
        Tools.Trace("OBJECTPOOL: INVALID ITEM INDEX: " + item.NodeIndex.ToString());
      else if (!this.isActive[item.NodeIndex])
      {
        Tools.Trace("OBJECTPOOL: ITEM INDEX ALREADY INACTIVE: " + item.NodeIndex.ToString());
      }
      else
      {
        this.isActive[item.NodeIndex] = false;
        this.Available.Enqueue(item.NodeIndex);
      }
    }

    public void SetItemValue(AdvancedObjectPool<T>.Node item)
    {
      if (item.NodeIndex < 0 || item.NodeIndex > this.Pool.Length)
        Tools.Trace("OBJECTPOOL: INVALID ITEM INDEX: " + item.NodeIndex.ToString());
      else
        this.Pool[item.NodeIndex].Item = item.Item;
    }

    public IEnumerator<T> GetEnumerator()
    {
      foreach (AdvancedObjectPool<T>.Node node in this.Pool)
      {
        if (this.isActive[node.NodeIndex])
          yield return node.Item;
      }
    }

    public IEnumerable<AdvancedObjectPool<T>.Node> ActiveNodes
    {
      get
      {
        foreach (AdvancedObjectPool<T>.Node activeNode in this.Pool)
        {
          if (this.isActive[activeNode.NodeIndex])
            yield return activeNode;
        }
      }
    }

    public IEnumerable<AdvancedObjectPool<T>.Node> AllNodes
    {
      get
      {
        foreach (AdvancedObjectPool<T>.Node allNode in this.Pool)
          yield return allNode;
      }
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct Node
    {
      public int NodeIndex;
      public T Item;
    }
  }
}
