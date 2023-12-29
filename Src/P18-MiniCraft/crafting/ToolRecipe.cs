// Decompiled with JetBrains decompiler
// Type: GameManager.crafting.ToolRecipe
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.entity;
using GameManager.item;
using GameManager.screen;

#nullable disable
namespace GameManager.crafting
{
  public class ToolRecipe : Recipe
  {
    private ToolType type;
    private int level;

    public ToolRecipe(ToolType type, int level)
      : base((Item) new ToolItem(type, level))
    {
      this.type = type;
      this.level = level;
    }

    public override void Craft(Player player)
    {
      player.PlayerInventory.Add(0, (ListItem) new ToolItem(this.type, this.level));
    }
  }
}
