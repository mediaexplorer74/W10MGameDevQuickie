// Decompiled with JetBrains decompiler
// Type: GameManager.crafting.Recipe
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.gfx;
using GameManager.item;
using GameManager.item.resource;
using GameManager.screen;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameManager.crafting
{
  public abstract class Recipe : ListItem
  {
    public List<Item> Costs = new List<Item>();
    public bool CanCraft;
    public Item ResultTemplate;

    public Recipe(Item resultTemplate) => this.ResultTemplate = resultTemplate;

    public Recipe AddCost(Resource resource, int count)
    {
      this.Costs.Add((Item) new ResourceItem(resource, count));
      return this;
    }

    public void CheckCanCraft(Player player)
    {
      for (int index = 0; index < this.Costs.Count; ++index)
      {
        Item cost = this.Costs[index];
        if (cost is ResourceItem)
        {
          ResourceItem resourceItem = (ResourceItem) cost;
          if (!player.PlayerInventory.HasResources(resourceItem.MyResource, resourceItem.Count))
          {
            this.CanCraft = false;
            return;
          }
        }
      }
      this.CanCraft = true;
    }

    public void RenderInventory(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.ResultTemplate.GetSprite(), this.ResultTemplate.GetColor(), 0);
      int col = this.CanCraft ? Color.Get(-1, 555, 555, 555) : Color.Get(-1, 222, 222, 222);
      Font.Draw(this.ResultTemplate.GetName(), screen, x + 8, y, col);
    }

    public abstract void Craft(Player player);

    public void DeductCost(Player player)
    {
      for (int index = 0; index < this.Costs.Count; ++index)
      {
        Item cost = this.Costs[index];
        if (cost is ResourceItem)
        {
          ResourceItem resourceItem = (ResourceItem) cost;
          player.PlayerInventory.RemoveResource(resourceItem.MyResource, resourceItem.Count);
        }
      }
    }

    public void SaveToWriter(Game1 game, BinaryWriter writer)
    {
    }
  }
}
