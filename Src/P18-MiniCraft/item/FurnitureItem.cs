// Decompiled with JetBrains decompiler
// Type: GameManager.item.FurnitureItem


using GameManager.entity;
using GameManager.gfx;
using GameManager.level;
using GameManager.level.tile;
using System.IO;

#nullable disable
namespace GameManager.item
{
  public class FurnitureItem : Item
  {
    public Furniture MyFurniture;
    public bool Placed;

        public FurnitureItem(Furniture furniture)
        { 
            this.MyFurniture = furniture; 
        }

    public FurnitureItem(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.Placed);
      this.MyFurniture.SaveToWriter(game, writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.Placed = reader.ReadBoolean();
      Entity entity = Entity.NewEntityFromReader(game, reader);
      if (entity is Furniture)
        this.MyFurniture = (Furniture) entity;
      else
        this.MyFurniture = (Furniture) null;
    }

        public override int GetColor()
        {
            return this.MyFurniture.Col;
        }

        public override int GetSprite()
        {
            return this.MyFurniture.Sprite + 320;
        }

        public override void RenderIcon(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.GetSprite(), this.GetColor(), 0);
    }

    public override void RenderInventory(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.GetSprite(), this.GetColor(), 0);
      Font.Draw(this.MyFurniture.Name, screen, x + 8, y, Color.Get(-1, 555, 555, 555));
    }

    public override void OnTake(ItemEntity itemEntity)
    {
    }

    public override bool CanAttack() => false;

    public override bool InteractOn(
      Tile tile,
      Level level,
      int xt,
      int yt,
      Player player,
      int attackDir)
    {
      if (!tile.MayPass(level, xt, yt, (Entity) this.MyFurniture))
        return false;
      this.MyFurniture.X = xt * 16 + 8;
      this.MyFurniture.Y = yt * 16 + 8;
      level.Add((Entity) this.MyFurniture);
      this.Placed = true;
      return true;
    }

    public override bool IsDepleted() => this.Placed;

    public override string GetName() => this.MyFurniture.Name;

    public override bool Matches(Item item)
    {
      return item is FurnitureItem 
                && (object) ((FurnitureItem) item).MyFurniture.GetType() 
                == (object) this.MyFurniture.GetType();
    }
  }
}
