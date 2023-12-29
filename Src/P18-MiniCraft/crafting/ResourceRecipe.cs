// Decompiled with JetBrains decompiler
// Type: GameManager.crafting.ResourceRecipe
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.item;
using GameManager.item.resource;
using GameManager.screen;

#nullable disable
namespace GameManager.crafting
{
  public class ResourceRecipe : Recipe
  {
    private Resource resource;

    public ResourceRecipe(Resource resource)
      : base((Item) new ResourceItem(resource, 1))
    {
      this.resource = resource;
    }

    public override void Craft(Player player)
    {
      player.PlayerInventory.Add(0, (ListItem) new ResourceItem(this.resource, 1));
    }
  }
}
