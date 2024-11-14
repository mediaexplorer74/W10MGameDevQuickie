// Decompiled with JetBrains decompiler
// Type: GameEngine.ObjectPool`1
// Assembly: GameEngineLibrary_1_4_6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0386BF57-8AD4-41EC-9726-4DD69BA8B0D9
// Assembly location: C:\Users\Admin\Desktop\RE\8bitInvaders\GameEngineLibrary_1_4_6.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace GameEngine
{
  public class ObjectPool<T> : IEnumerable<T>, IEnumerable where T : new()
  {
    private ObjectPool<T>.Node[] NodePool;
    private bool[] isActive;
    private Queue<int> Available;

    public int AvailableCount => this.Available.Count;

    public int ActiveCount => this.NodePool.Length - this.Available.Count;

    public int Capacity => this.NodePool.Length;

    public ObjectPool(int objectCount)
    {
      this.NodePool = new ObjectPool<T>.Node[objectCount];
      this.isActive = new bool[objectCount];
      this.Available = new Queue<int>(objectCount);
      for (int index = 0; index < objectCount; ++index)
      {
        this.NodePool[index] = new ObjectPool<T>.Node();
        this.NodePool[index].NodeIndex = index;
        this.NodePool[index].Item = new T();
        this.isActive[index] = false;
        this.Available.Enqueue(index);
      }
    }

    public void Clear()
    {
      this.Available.Clear();
      for (int index = 0; index < this.NodePool.Length; ++index)
      {
        this.isActive[index] = false;
        this.Available.Enqueue(index);
      }
    }

    public ObjectPool<T>.Node Get()
    {
      int index = this.Available.Dequeue();
      this.isActive[index] = true;
      return this.NodePool[index];
    }

    public void Return(ObjectPool<T>.Node item)
    {
      if (item.NodeIndex < 0 || item.NodeIndex > this.NodePool.Length)
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

    public void SetItemValue(ObjectPool<T>.Node item)
    {
      if (item.NodeIndex < 0 || item.NodeIndex > this.NodePool.Length)
        Tools.Trace("OBJECTPOOL: INVALID ITEM INDEX: " + item.NodeIndex.ToString());
      else
        this.NodePool[item.NodeIndex].Item = item.Item;
    }

    public IEnumerator<T> GetEnumerator()
    {
      foreach (ObjectPool<T>.Node node in this.NodePool)
      {
        if (this.isActive[node.NodeIndex])
          yield return node.Item;
      }
    }

    public IEnumerable<ObjectPool<T>.Node> ActiveNodes
    {
      get
      {
        foreach (ObjectPool<T>.Node activeNode in this.NodePool)
        {
          if (this.isActive[activeNode.NodeIndex])
            yield return activeNode;
        }
      }
    }

    public IEnumerable<ObjectPool<T>.Node> AllNodes
    {
      get
      {
        foreach (ObjectPool<T>.Node allNode in this.NodePool)
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
