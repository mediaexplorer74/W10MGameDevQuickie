// Decompiled with JetBrains decompiler
// Type: GameManager.screen.CraftingMenu


using GameManager.crafting;
using GameManager.entity;
using GameManager.gfx;
using GameManager.item;
using GameManager.sound;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace GameManager.screen
{
  public class CraftingMenu : Menu
  {
    private Player player;
    private int selected;
    private Crafting.CraftingType craftingType;
    private List<Recipe> recipes;

    public CraftingMenu(Crafting.CraftingType craftingType, Player player)
    {
      this.player = player;
      this.craftingType = craftingType;
      this.getRecipesFromCraftingType();
    }

    public CraftingMenu(Game1 game, BinaryReader reader)
    {
      this.Init(game, game.Input);
      this.player = game.ActivePlayer;
      this.LoadFromReader(game, reader);
    }

    public static int CompareRecipe(Recipe r1, Recipe r2)
    {
      if (r1.CanCraft && !r2.CanCraft)
        return -1;
      return !r1.CanCraft && r2.CanCraft ? 1 : 0;
    }

    public override void Tick()
    {
      if (InputHandler.Menu.Clicked)
        this.game.SetMenu((Menu) null);

      if (InputHandler.Up.Clicked)
        --this.selected;

      if (InputHandler.Down.Clicked)
        ++this.selected;
      int count = this.recipes.Count;

      if (count == 0)
        this.selected = 0;

      if (this.selected < 0)
        this.selected += count;

      if (this.selected >= count)
        this.selected -= count;

      if (!InputHandler.Attack.Clicked || count <= 0)
        return;

      Recipe recipe = this.recipes[this.selected];
      recipe.CheckCanCraft(this.player);
      if (recipe.CanCraft)
      {
        recipe.DeductCost(this.player);
        recipe.Craft(this.player);
        Sound.Craft.Play();
      }
      for (int index = 0; index < this.recipes.Count; ++index)
        this.recipes[index].CheckCanCraft(this.player);
    }

    public override void Render(Screen screen)
    {
      Font.RenderFrame(screen, "Have", 12, 1, 19, 3);
      Font.RenderFrame(screen, "Cost", 12, 4, 19, 11);
      Font.RenderFrame(screen, "Crafting", 0, 1, 11, 11);
      List<ListItem> listItems = new List<ListItem>(this.recipes.Count);
      for (int index = 0; index < this.recipes.Count; ++index)
        listItems.Add((ListItem) this.recipes[index]);
      this.RenderItemList(screen, 0, 1, 11, 11, listItems, this.selected);
      if (this.recipes.Count <= 0)
        return;
      Recipe recipe = this.recipes[this.selected];
      int num1 = this.player.PlayerInventory.Count(recipe.ResultTemplate);
      int xp = 104;
      screen.Render(xp, 16, recipe.ResultTemplate.GetSprite(), recipe.ResultTemplate.GetColor(), 0);
      Font.Draw(string.Empty + (object) num1, screen, xp + 8, 16, Color.Get(-1, 555, 555, 555));
      List<Item> costs = recipe.Costs;
      for (int index = 0; index < costs.Count; ++index)
      {
        Item obj = costs[index];
        int num2 = (5 + index) * 8;
        screen.Render(xp, num2, obj.GetSprite(), obj.GetColor(), 0);
        int num3 = 1;
        if (obj is ResourceItem)
          num3 = ((ResourceItem) obj).Count;
        int num4 = this.player.PlayerInventory.Count(obj);
        int col = Color.Get(-1, 555, 555, 555);
        if (num4 < num3)
          col = Color.Get(-1, 222, 222, 222);
        if (num4 > 99)
          num4 = 99;
        Font.Draw(string.Empty + (object) num3 + "/" + (object) num4, screen, xp + 8, num2, col);
      }
    }

    public override void SaveToWriter(BinaryWriter writer)
    {
      base.SaveToWriter(writer);
      writer.Write(this.selected);
      writer.Write((short) this.craftingType);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.selected = reader.ReadInt32();
      this.craftingType = (Crafting.CraftingType) reader.ReadInt16();
      this.getRecipesFromCraftingType();
    }

    private void getRecipesFromCraftingType()
    {
      switch (this.craftingType)
      {
        case Crafting.CraftingType.anvil:
          this.recipes = new List<Recipe>((IEnumerable<Recipe>) Crafting.AnvilRecipes);
          break;
        case Crafting.CraftingType.oven:
          this.recipes = new List<Recipe>((IEnumerable<Recipe>) Crafting.OvenRecipes);
          break;
        case Crafting.CraftingType.furnace:
          this.recipes = new List<Recipe>((IEnumerable<Recipe>) Crafting.FurnaceRecipes);
          break;
        case Crafting.CraftingType.workbench:
          this.recipes = new List<Recipe>((IEnumerable<Recipe>) Crafting.WorkbenchRecipes);
          break;
        default:
          this.recipes = new List<Recipe>();
          break;
      }
      for (int index = 0; index < this.recipes.Count; ++index)
        this.recipes[index].CheckCanCraft(this.player);
      this.recipes.Sort(new Comparison<Recipe>(CraftingMenu.CompareRecipe));
    }
  }
}
