// Decompiled with JetBrains decompiler
// Type: GameManager.crafting.FurnitureRecipe
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.item;
using GameManager.screen;
using System;

#nullable disable
namespace GameManager.crafting
{
  public class FurnitureRecipe : Recipe
  {
    private Type clazz;

    public FurnitureRecipe(Type clazz)
      : base((Item) new FurnitureItem((Furniture) Activator.CreateInstance(clazz)))
    {
      this.clazz = clazz;
    }

    public override void Craft(Player player)
    {
      try
      {
        player.PlayerInventory.Add(0, (ListItem) new FurnitureItem((Furniture) Activator.CreateInstance(this.clazz)));
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
