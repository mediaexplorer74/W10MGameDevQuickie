// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Slime


using GameManager.gfx;
using GameManager.item;
using GameManager.item.resource;
using Microsoft.Xna.Framework;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Slime : Mob
  {
    private int xa;
    private int ya;
    private int jumpTime;
    private int lvl;

    public Slime(int lvl)
    {
      this.lvl = lvl;
      this.X = this.random.Next(1024);
      this.Y = this.random.Next(1024);
      this.Health = this.MaxHealth = lvl * lvl * 5;
    }

    public Slime(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.xa);
      writer.Write(this.ya);
      writer.Write(this.jumpTime);
      writer.Write(this.lvl);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.xa = reader.ReadInt32();
      this.ya = reader.ReadInt32();
      this.jumpTime = reader.ReadInt32();
      this.lvl = reader.ReadInt32();
    }

    public override void Tick()
    {
      base.Tick();
      int num1 = 1;
      if ((!this.Move(this.xa * num1, this.ya * num1) 
                || this.random.Next(40) == 0) && this.jumpTime <= -10)
      {
        this.xa = this.random.Next(3) - 1;
        this.ya = this.random.Next(3) - 1;
        if (this.MyLevel.MyPlayer != null)
        {
          int num2 = this.MyLevel.MyPlayer.X - this.X;
          int num3 = this.MyLevel.MyPlayer.Y - this.Y;
          if (num2 * num2 + num3 * num3 < 2500)
          {
            if (num2 < 0)
              this.xa = -1;
            if (num2 > 0)
              this.xa = 1;
            if (num3 < 0)
              this.ya = -1;
            if (num3 > 0)
              this.ya = 1;
          }
        }
        if (this.xa != 0 || this.ya != 0)
          this.jumpTime = 10;
      }
      --this.jumpTime;
      if (this.jumpTime != 0)
        return;
      this.xa = this.ya = 0;
    }

    public override void Render(Screen screen)
    {
      int num1 = 0;
      int num2 = 18;
      int xp = this.X - 8;
      int yp = this.Y - 11;
      if (this.jumpTime > 0)
      {
        num1 += 2;
        yp -= 4;
      }
      int colors = gfx.Color.Get(-1, 10, 252, 555);
      if (this.lvl == 2)
        colors = gfx.Color.Get(-1, 100, 522, 555);
      if (this.lvl == 3)
        colors = gfx.Color.Get(-1, 111, 444, 555);
      if (this.lvl == 4)
        colors = gfx.Color.Get(-1, 0, 111, 224);
      if (this.HurtTime > 0)
        colors = gfx.Color.Get(-1, 555, 555, 555);
      screen.Render(xp, yp, num1 + num2 * 32, colors, 0);
      screen.Render(xp + 8, yp, num1 + 1 + num2 * 32, colors, 0);
      screen.Render(xp, yp + 8, num1 + (num2 + 1) * 32, colors, 0);
      screen.Render(xp + 8, yp + 8, num1 + 1 + (num2 + 1) * 32, colors, 0);
    }

    public override void TouchedBy(Entity entity)
    {
      if (!(entity is Player))
        return;
      entity.Hurt((Mob) this, this.lvl, this.Dir);
    }

    protected override void Die()
    {
      base.Die();
      int num = this.random.Next(2) + 1;
      for (int index = 0; index < num; ++index)
        this.MyLevel.Add((Entity) new ItemEntity((Item) new ResourceItem(Resource.Slime),
            this.X + this.random.Next(11) - 5, this.Y + this.random.Next(11) - 5));
      if (this.MyLevel.MyPlayer == null)
        return;
      this.MyLevel.MyPlayer.Score += 25 * this.lvl;
    }
  }
}
