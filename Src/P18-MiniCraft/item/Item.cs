// Decompiled with JetBrains decompiler
// Type: GameManager.item.Item


using GameManager.entity;
using GameManager.gfx;
using GameManager.level;
using GameManager.level.tile;
using GameManager.screen;
using System;
using System.IO;

#nullable disable
namespace GameManager.item
{
  public class Item : ListItem
  {
    public Item()
    {
    }

    public Item(Game1 game, BinaryReader reader)
    {
        this.LoadFromReader(game, reader);
    }

    public void SaveItemTypeToWriter(BinaryWriter writer)
    {
      switch (this)
      {
        case FurnitureItem _:
          writer.Write((short) 1);
          break;
        case PowerGloveItem _:
          writer.Write((short) 2);
          break;
        case ResourceItem _:
          writer.Write((short) 3);
          break;
        case ToolItem _:
          writer.Write((short) 4);
          break;
        default:
          throw new Exception("unknown ItemType " + this.GetType().ToString());
      }
    }

    public virtual void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      this.SaveItemTypeToWriter(writer);
    }

    public virtual void LoadFromReader(Game1 game, BinaryReader reader)
    {
    }

    public static Item NewItemFromReader(Game1 game, BinaryReader reader)
    {
      Item.ItemType itemType = (Item.ItemType) reader.ReadInt16();
      switch (itemType)
      {
        case Item.ItemType.Furniture:
          return (Item) new FurnitureItem(game, reader);

        case Item.ItemType.PowerGlove:
          return (Item) new PowerGloveItem(game, reader);

        case Item.ItemType.Resource:
          return (Item) new ResourceItem(game, reader);

        case Item.ItemType.Tool:
          return (Item) new ToolItem(game, reader);

        default:
          throw new Exception("unknown Item " + itemType.ToString("F"));
      }
    }

    public virtual int GetColor() => 0;

    public virtual int GetSprite() => 0;

    public virtual void OnTake(ItemEntity itemEntity)
    {
    }

    public virtual void RenderInventory(Screen screen, int x, int y)
    {
    }

        public virtual bool Interact(Player player, Entity entity, int attackDir)
        {
            return false;
        }

        public virtual void RenderIcon(Screen screen, int x, int y)
    {
    }

    public virtual bool InteractOn(
      Tile tile,
      Level level,
      int xt,
      int yt,
      Player player,
      int attackDir)
    {
      return false;
    }

    public virtual bool IsDepleted()
    {
        return false;
    }

    public virtual bool CanAttack()
    {
        return false;
    }

    public virtual int GetAttackDamageBonus(Entity e)
    {
        return 0;
    }

    public virtual string GetName()
    {
        return string.Empty;
    }

    public virtual bool Matches(Item item)
    {
        return (object)item.GetType() == (object)this.GetType();
    }

    public enum ItemType
    {
      UNKNOWN,
      Furniture,
      PowerGlove,
      Resource,
      Tool,
    }
  }
}
