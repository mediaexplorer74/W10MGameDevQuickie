// Decompiled with JetBrains decompiler
// Type: GameManager.entity.ItemEntity


using GameManager.gfx;
using GameManager.item;
using GameManager.sound;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class ItemEntity : Entity
  {
    public double Xa;
    public double Ya;
    public double Za;
    public double Xx;
    public double Yy;
    public double Zz;
    public Item MyItem;
    public int HurtTime;
    protected int walkDist;
    protected int dir;
    protected int xKnockback;
    protected int yKnockback;
    private int lifeTime;
    private int time;

    public ItemEntity(Item item, int x, int y)
    {
      this.MyItem = item;
      this.Xx = (double) (this.X = x);
      this.Yy = (double) (this.Y = y);
      this.Xr = 3;
      this.Yr = 3;
      this.Zz = 2.0;
      this.Xa = (this.random.NextDouble() * 2.0 - 1.0) * 0.3;
      this.Ya = (this.random.NextDouble() * 2.0 - 1.0) * 0.2;
      this.Za = this.random.NextDouble() * 0.7 + 1.0;
      this.lifeTime = 600 + this.random.Next(60);
    }

    public ItemEntity(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.lifeTime);
      writer.Write(this.walkDist);
      writer.Write(this.dir);
      writer.Write(this.HurtTime);
      writer.Write(this.xKnockback);
      writer.Write(this.yKnockback);
      writer.Write(this.Xa);
      writer.Write(this.Ya);
      writer.Write(this.Za);
      writer.Write(this.Xx);
      writer.Write(this.Yy);
      writer.Write(this.Zz);
      writer.Write(this.time);

      this.MyItem.SaveToWriter(game, writer);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.lifeTime = reader.ReadInt32();
      this.walkDist = reader.ReadInt32();
      this.dir = reader.ReadInt32();
      this.HurtTime = reader.ReadInt32();
      this.xKnockback = reader.ReadInt32();
      this.yKnockback = reader.ReadInt32();
      this.Xa = reader.ReadDouble();
      this.Ya = reader.ReadDouble();
      this.Za = reader.ReadDouble();
      this.Xx = reader.ReadDouble();
      this.Yy = reader.ReadDouble();
      this.Zz = reader.ReadDouble();
      this.time = reader.ReadInt32();
      this.MyItem = Item.NewItemFromReader(game, reader);
    }

    public override void Tick()
    {
      ++this.time;
      if (this.time >= this.lifeTime)
      {
        this.Remove();
      }
      else
      {
        this.Xx += this.Xa;
        this.Yy += this.Ya;
        this.Zz += this.Za;
        if (this.Zz < 0.0)
        {
          this.Zz = 0.0;
          this.Za *= -0.5;
          this.Xa *= 0.6;
          this.Ya *= 0.6;
        }
        this.Za -= 0.15;
        int x = this.X;
        int y = this.Y;
        int xx = (int) this.Xx;
        int yy = (int) this.Yy;
        int num1 = xx - this.X;
        int num2 = yy - this.Y;
        this.Move(xx - this.X, yy - this.Y);
        int num3 = this.X - x;
        int num4 = this.Y - y;
        this.Xx += (double) (num3 - num1);
        this.Yy += (double) (num4 - num2);
        if (this.HurtTime <= 0)
          return;
        --this.HurtTime;
      }
    }

    public override bool IsBlockableBy(Mob mob) => false;

    public override void Render(Screen screen)
    {
      if (this.time >= this.lifeTime - 120 && this.time / 6 % 2 == 0)
        return;
      screen.Render(this.X - 4, this.Y - 4, this.MyItem.GetSprite(),
          gfx.Color.Get(-1, 0, 0, 0), 0);
      screen.Render(this.X - 4, this.Y - 4 - (int) this.Zz, this.MyItem.GetSprite(),
          this.MyItem.GetColor(), 0);
    }

    public override void TouchedBy(Entity entity)
    {
      if (this.time <= 30)
        return;
      entity.TouchItem(this);
    }

    public void Take(Player player)
    {
      Sound.Pickup.Play();
      ++player.Score;
      this.MyItem.OnTake(this);
      this.Remove();
    }
  }
}
