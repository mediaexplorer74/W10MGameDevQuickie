// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Zombie
// Assembly: GameManager, Version=1.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 41710B6C-AEBD-459C-B6C4-49BC31BEE4AD
// Assembly location: C:\Users\Admin\Desktop\RE\GameManager.dll

using GameManager.gfx;
using GameManager.item;
using GameManager.item.resource;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Zombie : Mob
  {
    private int xa;
    private int ya;
    private int lvl;
    private int randomWalkTime;

    public Zombie(int lvl)
    {
      this.lvl = lvl;
      this.X = this.random.Next(1024);
      this.Y = this.random.Next(1024);
      this.Health = this.MaxHealth = lvl * lvl * 10;
    }

    public Zombie(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.xa);
      writer.Write(this.ya);
      writer.Write(this.lvl);
      writer.Write(this.randomWalkTime);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.xa = reader.ReadInt32();
      this.ya = reader.ReadInt32();
      this.lvl = reader.ReadInt32();
      this.randomWalkTime = reader.ReadInt32();
    }

    public override void Tick()
    {
      base.Tick();
      if (this.MyLevel.MyPlayer != null && this.randomWalkTime == 0)
      {
        int num1 = this.MyLevel.MyPlayer.X - this.X;
        int num2 = this.MyLevel.MyPlayer.Y - this.Y;
        if (num1 * num1 + num2 * num2 < 2500)
        {
          this.xa = 0;
          this.ya = 0;
          if (num1 < 0)
            this.xa = -1;
          if (num1 > 0)
            this.xa = 1;
          if (num2 < 0)
            this.ya = -1;
          if (num2 > 0)
            this.ya = 1;
        }
      }
      int num = this.TickTime & 1;
      if (!this.Move(this.xa * num, this.ya * num) || this.random.Next(200) == 0)
      {
        this.randomWalkTime = 60;
        this.xa = (this.random.Next(3) - 1) * this.random.Next(2);
        this.ya = (this.random.Next(3) - 1) * this.random.Next(2);
      }
      if (this.randomWalkTime <= 0)
        return;
      --this.randomWalkTime;
    }

    public override void Render(Screen screen)
    {
      int num1 = 0;
      int num2 = 14;
      int bits1 = this.walkDist >> 3 & 1;
      int bits2 = this.walkDist >> 3 & 1;
      if (this.Dir == 1)
        num1 += 2;
      if (this.Dir > 1)
      {
        bits1 = 0;
        bits2 = this.walkDist >> 4 & 1;
        if (this.Dir == 2)
          bits1 = 1;
        num1 += 4 + (this.walkDist >> 3 & 1) * 2;
      }
      int num3 = this.X - 8;
      int yp = this.Y - 11;
      int colors = gfx.Color.Get(-1, 10, 252, 50);
      
      if (this.lvl == 2)
        colors = gfx.Color.Get(-1, 100, 522, 50);
      if (this.lvl == 3)
        colors = gfx.Color.Get(-1, 111, 444, 50);
      if (this.lvl == 4)
        colors = gfx.Color.Get(-1, 0, 111, 20);
      if (this.HurtTime > 0)
        colors = gfx.Color.Get(-1, 555, 555, 555);

      screen.Render(num3 + 8 * bits1, yp, num1 + num2 * 32, colors, bits1);
      screen.Render(num3 + 8 - 8 * bits1, yp, num1 + 1 + num2 * 32, colors, bits1);
      screen.Render(num3 + 8 * bits2, yp + 8, num1 + (num2 + 1) * 32, colors, bits2);
      screen.Render(num3 + 8 - 8 * bits2, yp + 8, num1 + 1 + (num2 + 1) * 32, colors, bits2);
    }

    public override void TouchedBy(Entity entity)
    {
      if (!(entity is Player))
        return;
      entity.Hurt((Mob) this, this.lvl + 1, this.Dir);
    }

    protected override void Die()
    {
      base.Die();
      int num = this.random.Next(2) + 1;
      for (int index = 0; index < num; ++index)
        this.MyLevel.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Cloth), this.X + this.random.Next(11) - 5, this.Y + this.random.Next(11) - 5));
      if (this.MyLevel.MyPlayer == null)
        return;
      this.MyLevel.MyPlayer.Score += 50 * this.lvl;
    }
  }
}
