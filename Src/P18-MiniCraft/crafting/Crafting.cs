// Decompiled with JetBrains decompiler
// Type: GameManager.crafting.Crafting
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.item;
using GameManager.item.resource;
using System.Collections.Generic;

#nullable disable
namespace GameManager.crafting
{
  public static class Crafting
  {
    public static readonly List<Recipe> AnvilRecipes;
    public static readonly List<Recipe> OvenRecipes;
    public static readonly List<Recipe> FurnaceRecipes;
    public static readonly List<Recipe> WorkbenchRecipes = new List<Recipe>();

    static Crafting()
    {
      Crafting.WorkbenchRecipes.Add(new FurnitureRecipe(typeof (Lantern)).AddCost(Resource.Wood, 5).AddCost(Resource.Slime, 10).AddCost(Resource.Glass, 4));
      Crafting.WorkbenchRecipes.Add(new FurnitureRecipe(typeof (Oven)).AddCost(Resource.Stone, 15));
      Crafting.WorkbenchRecipes.Add(new FurnitureRecipe(typeof (Furnace)).AddCost(Resource.Stone, 20));
      Crafting.WorkbenchRecipes.Add(new FurnitureRecipe(typeof (Workbench)).AddCost(Resource.Wood, 20));
      Crafting.WorkbenchRecipes.Add(new FurnitureRecipe(typeof (Chest)).AddCost(Resource.Wood, 20));
      Crafting.WorkbenchRecipes.Add(new FurnitureRecipe(typeof (Anvil)).AddCost(Resource.IronIngot, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Sword, 0).AddCost(Resource.Wood, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Axe, 0).AddCost(Resource.Wood, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Hoe, 0).AddCost(Resource.Wood, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Pickaxe, 0).AddCost(Resource.Wood, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Shovel, 0).AddCost(Resource.Wood, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Sword, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Axe, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Hoe, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Pickaxe, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
      Crafting.WorkbenchRecipes.Add(new ToolRecipe(ToolType.Shovel, 1).AddCost(Resource.Wood, 5).AddCost(Resource.Stone, 5));
      Crafting.AnvilRecipes = new List<Recipe>();
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Sword, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Axe, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Hoe, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Pickaxe, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Shovel, 2).AddCost(Resource.Wood, 5).AddCost(Resource.IronIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Sword, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Axe, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Hoe, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Pickaxe, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Shovel, 3).AddCost(Resource.Wood, 5).AddCost(Resource.GoldIngot, 5));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Sword, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Axe, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Hoe, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Pickaxe, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
      Crafting.AnvilRecipes.Add(new ToolRecipe(ToolType.Shovel, 4).AddCost(Resource.Wood, 5).AddCost(Resource.Gem, 50));
      Crafting.FurnaceRecipes = new List<Recipe>();
      Crafting.FurnaceRecipes.Add(new ResourceRecipe(Resource.IronIngot).AddCost(Resource.IronOre, 4).AddCost(Resource.Coal, 1));
      Crafting.FurnaceRecipes.Add(new ResourceRecipe(Resource.GoldIngot).AddCost(Resource.GoldOre, 4).AddCost(Resource.Coal, 1));
      Crafting.FurnaceRecipes.Add(new ResourceRecipe(Resource.Glass).AddCost(Resource.Sand, 4).AddCost(Resource.Coal, 1));
      Crafting.OvenRecipes = new List<Recipe>();
      Crafting.OvenRecipes.Add(new ResourceRecipe(Resource.Bread).AddCost(Resource.Wheat, 4));
    }

    public enum CraftingType
    {
      anvil,
      oven,
      furnace,
      workbench,
    }
  }
}
