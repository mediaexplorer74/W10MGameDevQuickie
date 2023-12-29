// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Furniture
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.gfx;
using GameManager.item;
using GameManager.screen;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Furniture : Entity
  {
    public int Col;
    public int Sprite;
    public string Name;
    private Player shouldTake;
    private int pushTime;
    private int pushDir = -1;

    public Furniture(string name)
    {
      this.Name = name;
      this.Xr = 3;
      this.Yr = 3;
    }

    public Furniture(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.pushTime);
      writer.Write(this.pushDir);
      writer.Write(this.Col);
      writer.Write(this.Sprite);
      writer.Write(this.Name);
      writer.Write(this.shouldTake != null);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.pushTime = reader.ReadInt32();
      this.pushDir = reader.ReadInt32();
      this.Col = reader.ReadInt32();
      this.Sprite = reader.ReadInt32();
      this.Name = reader.ReadString();
      if (reader.ReadBoolean())
        this.shouldTake = this.MyLevel.MyPlayer;
      else
        this.shouldTake = (Player) null;
    }

    public override void Tick()
    {
      if (this.shouldTake != null)
      {
        if (this.shouldTake.ActiveItem is PowerGloveItem)
        {
          this.Remove();
          this.shouldTake.PlayerInventory.Add(0, (ListItem) this.shouldTake.ActiveItem);
          this.shouldTake.ActiveItem = (Item) new FurnitureItem(this);
        }
        this.shouldTake = (Player) null;
      }
      if (this.pushDir == 0)
        this.Move(0, 1);
      if (this.pushDir == 1)
        this.Move(0, -1);
      if (this.pushDir == 2)
        this.Move(-1, 0);
      if (this.pushDir == 3)
        this.Move(1, 0);
      this.pushDir = -1;
      if (this.pushTime <= 0)
        return;
      --this.pushTime;
    }

    public override void Render(Screen screen)
    {
      screen.Render(this.X - 8, this.Y - 8 - 4, this.Sprite * 2 + 256, this.Col, 0);
      screen.Render(this.X, this.Y - 8 - 4, this.Sprite * 2 + 256 + 1, this.Col, 0);
      screen.Render(this.X - 8, this.Y - 4, this.Sprite * 2 + 256 + 32, this.Col, 0);
      screen.Render(this.X, this.Y - 4, this.Sprite * 2 + 256 + 33, this.Col, 0);
    }

    public override bool Blocks(Entity e) => true;

    public override void TouchedBy(Entity entity)
    {
      if (!(entity is Player) || this.pushTime != 0)
        return;
      this.pushDir = ((Mob) entity).Dir;
      this.pushTime = 10;
    }

    public void Take(Player player) => this.shouldTake = player;
  }
}
