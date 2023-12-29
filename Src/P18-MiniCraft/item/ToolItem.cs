// Decompiled with JetBrains decompiler
// Type: GameManager.item.ToolItem


using GameManager.entity;
using GameManager.gfx;
using System;
using System.IO;

#nullable disable
namespace GameManager.item
{
  public class ToolItem : Item
  {
    public const int MAX_LEVEL = 5;
    public static string[] LEVEL_NAMES = new string[5]
    {
      "Wood",
      "Rock",
      "Iron",
      "Gold",
      "Gem"
    };
    public static int[] LEVEL_COLORS = new int[5]
    {
      Color.Get(-1, 100, 321, 431),
      Color.Get(-1, 100, 321, 111),
      Color.Get(-1, 100, 321, 555),
      Color.Get(-1, 100, 321, 550),
      Color.Get(-1, 100, 321, 55)
    };
    public ToolType MyType;
    public int ToolLevel;
    private Random random = new Random();

    public ToolItem(ToolType type, int level)
    {
      this.MyType = type;
      this.ToolLevel = level;
    }

    public ToolItem(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.ToolLevel);
      writer.Write(this.MyType.ToolSprite);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.ToolLevel = reader.ReadInt32();
      this.MyType = ToolType.GetToolFromSpriteNum(reader.ReadInt32());
    }

    public override int GetColor() => ToolItem.LEVEL_COLORS[this.ToolLevel];

    public override int GetSprite() => this.MyType.ToolSprite + 160;

    public override void RenderIcon(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.GetSprite(), this.GetColor(), 0);
    }

    public override void RenderInventory(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.GetSprite(), this.GetColor(), 0);
      Font.Draw(this.GetName(), screen, x + 8, y, Color.Get(-1, 555, 555, 555));
    }

    public override string GetName()
    {
      return ToolItem.LEVEL_NAMES[this.ToolLevel] + " " + this.MyType.Name;
    }

    public override void OnTake(ItemEntity itemEntity)
    {
    }

    public override bool CanAttack() => true;

    public override int GetAttackDamageBonus(Entity e)
    {
      if (this.MyType == ToolType.Axe)
        return (this.ToolLevel + 1) * 2 + this.random.Next(4);
      return this.MyType == ToolType.Sword ? (this.ToolLevel + 1) * 3 + this.random.Next(2 + this.ToolLevel * this.ToolLevel * 2) : 1;
    }

    public override bool Matches(Item item)
    {
      if (!(item is ToolItem))
        return false;
      ToolItem toolItem = (ToolItem) item;
      return toolItem.MyType == this.MyType && toolItem.ToolLevel == this.ToolLevel;
    }
  }
}
