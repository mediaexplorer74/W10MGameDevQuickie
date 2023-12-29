// Decompiled with JetBrains decompiler
// Type: GameManager.item.ResourceItem


using GameManager.entity;
using GameManager.gfx;
using GameManager.item.resource;
using GameManager.level;
using GameManager.level.tile;
using System.IO;

#nullable disable
namespace GameManager.item
{
  public class ResourceItem : Item
  {
    public Resource MyResource;
    public int Count = 1;

        public ResourceItem(Resource resource)
        {
            this.MyResource = resource;
        }

        public ResourceItem(Resource resource, int count)
    {
      this.MyResource = resource;
      this.Count = count;
    }

    public ResourceItem(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.Count);
      writer.Write(this.MyResource.Name);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.Count = reader.ReadInt32();
      this.MyResource = Resource.GetResourceByName(reader.ReadString());
    }

        public override int GetColor()
        {
            return this.MyResource.ResColor;
        }

        public override int GetSprite()
        {
            return this.MyResource.Sprite;
        }

        public override void RenderIcon(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.MyResource.Sprite, this.MyResource.ResColor, 0);
    }

    public override void RenderInventory(Screen screen, int x, int y)
    {
      screen.Render(x, y, this.MyResource.Sprite, this.MyResource.ResColor, 0);
      Font.Draw(this.MyResource.Name, screen, x + 32, y, Color.Get(-1, 555, 555, 555));
      int num = this.Count;
      if (num > 999)
        num = 999;
      Font.Draw(string.Empty + (object) num, screen, x + 8, y, Color.Get(-1, 444, 444, 444));
    }

    public override string GetName() => this.MyResource.Name;

    public override void OnTake(ItemEntity itemEntity)
    {
    }

    public override bool InteractOn(
      Tile tile,
      Level level,
      int xt,
      int yt,
      Player player,
      int attackDir)
    {
      if (!this.MyResource.interactOn(tile, level, xt, yt, player, attackDir))
        return false;
      --this.Count;
      return true;
    }

    public override bool IsDepleted()
    {
        return this.Count <= 0;
    }
  }
}
