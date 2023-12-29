// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Inventory

using GameManager.item;
using GameManager.item.resource;
using GameManager.screen;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Inventory
  {
    public List<ListItem> Items = new List<ListItem>();

    public Inventory()
    {
    }

    public Inventory(Game1 game, BinaryReader reader)
    {
        this.LoadFromReader(game, reader);
    }

    public void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      writer.Write(this.Items.Count);
      for (int index = 0; index < this.Items.Count; ++index)
        this.Items[index].SaveToWriter(game, writer);
    }

    public void LoadFromReader(Game1 game, BinaryReader reader)
    {
      int num = reader.ReadInt32();
      for (int index = 0; index < num; ++index)
        this.Items.Add((ListItem) Item.NewItemFromReader(game, reader));
    }

    public void SaveItemIndexToWriter(ListItem item, BinaryWriter writer)
    {
      writer.Write(this.Items.IndexOf(item));
    }

    public ListItem GetItemByIndex(int index)
    {
      return index >= 0 && index < this.Items.Count ? this.Items[index] : (ListItem) null;
    }

    public void Add(ListItem item)
    {
        this.Add(this.Items.Count, item);
    }

    public void Add(int slot, ListItem item)
    {
      if (item is ResourceItem)
      {
        ResourceItem resourceItem = (ResourceItem) item;
        ResourceItem resource = this.FindResource(resourceItem.MyResource);
        if (resource == null)
          this.Items.Insert(slot, (ListItem) resourceItem);
        else
          resource.Count += resourceItem.Count;
      }
      else
        this.Items.Insert(slot, item);
    }

    public bool HasResources(Resource r, int count)
    {
      ResourceItem resource = this.FindResource(r);
      return resource != null && resource.Count >= count;
    }

    public bool RemoveResource(Resource r, int count)
    {
      ResourceItem resource = this.FindResource(r);
      if (resource == null || resource.Count < count)
        return false;
      resource.Count -= count;
      if (resource.Count <= 0)
        this.Items.Remove((ListItem) resource);
      return true;
    }

    public int Count(Item item)
    {
      if (item is ResourceItem)
      {
        ResourceItem resource = this.FindResource(((ResourceItem) item).MyResource);
        return resource != null ? resource.Count : 0;
      }
      int num = 0;
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.Items[index] is Item && ((Item) this.Items[index]).Matches(item))
          ++num;
      }
      return num;
    }

    private ResourceItem FindResource(Resource resource)
    {
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.Items[index] is ResourceItem)
        {
          ResourceItem resource1 = (ResourceItem) this.Items[index];
          if (resource1.MyResource == resource)
            return resource1;
        }
      }
      return (ResourceItem) null;
    }
  }
}
