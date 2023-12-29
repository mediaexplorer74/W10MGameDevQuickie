// Decompiled with JetBrains decompiler
// Type: GameManager.entity.AirWizard


using GameManager.gfx;
using GameManager.sound;
using System;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class AirWizard : Mob
  {
    private int xa;
    private int ya;
    private int randomWalkTime;
    private int attackDelay;
    private int attackTime;
    private int attackType;

    public AirWizard()
    {
      this.X = this.random.Next(1024);
      this.Y = this.random.Next(1024);
      this.Health = this.MaxHealth = 2000;
    }

    public AirWizard(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.xa);
      writer.Write(this.ya);
      writer.Write(this.randomWalkTime);
      writer.Write(this.attackDelay);
      writer.Write(this.attackTime);
      writer.Write(this.attackType);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.xa = reader.ReadInt32();
      this.ya = reader.ReadInt32();
      this.randomWalkTime = reader.ReadInt32();
      this.attackDelay = reader.ReadInt32();
      this.attackTime = reader.ReadInt32();
      this.attackType = reader.ReadInt32();
    }

    public override void Tick()
    {
      base.Tick();
      if (this.attackDelay > 0)
      {
        this.Dir = (this.attackDelay - 45) / 4 % 4;
        this.Dir = this.Dir * 2 % 4 + this.Dir / 2;
        if (this.attackDelay < 45)
          this.Dir = 0;
        --this.attackDelay;
        if (this.attackDelay != 0)
          return;
        this.attackType = 0;
        if (this.Health < 1000)
          this.attackType = 1;
        if (this.Health < 200)
          this.attackType = 2;
        this.attackTime = 120;
      }
      else if (this.attackTime > 0)
      {
        --this.attackTime;
        double num1 = (double) this.attackTime * 0.25 * (double) (this.attackTime % 2 * 2 - 1);
        double num2 = 0.7 + (double) this.attackType * 0.2;
        this.MyLevel.Add((Entity) new Spark(this, Math.Cos(num1) * num2, Math.Sin(num1) * num2));
      }
      else
      {
        if (this.MyLevel.MyPlayer != null && this.randomWalkTime == 0)
        {
          int num3 = this.MyLevel.MyPlayer.X - this.X;
          int num4 = this.MyLevel.MyPlayer.Y - this.Y;
          if (num3 * num3 + num4 * num4 < 1024)
          {
            this.xa = 0;
            this.ya = 0;
            if (num3 < 0)
              this.xa = 1;
            if (num3 > 0)
              this.xa = -1;
            if (num4 < 0)
              this.ya = 1;
            if (num4 > 0)
              this.ya = -1;
          }
          else if (num3 * num3 + num4 * num4 > 6400)
          {
            this.xa = 0;
            this.ya = 0;
            if (num3 < 0)
              this.xa = -1;
            if (num3 > 0)
              this.xa = 1;
            if (num4 < 0)
              this.ya = -1;
            if (num4 > 0)
              this.ya = 1;
          }
        }
        int num5 = this.TickTime % 4 == 0 ? 0 : 1;
        if (!this.Move(this.xa * num5, this.ya * num5) || this.random.Next(100) == 0)
        {
          this.randomWalkTime = 30;
          this.xa = this.random.Next(3) - 1;
          this.ya = this.random.Next(3) - 1;
        }
        if (this.randomWalkTime <= 0)
          return;
        --this.randomWalkTime;
        if (this.MyLevel.MyPlayer == null || this.randomWalkTime != 0)
          return;
        int num6 = this.MyLevel.MyPlayer.X - this.X;
        int num7 = this.MyLevel.MyPlayer.Y - this.Y;
        if (this.random.Next(4) != 0 || num6 * num6 + num7 * num7 >= 2500
                    || this.attackDelay != 0 || this.attackTime != 0)
          return;
        this.attackDelay = 120;
      }
    }

    public override void Render(Screen screen)
    {
      int num1 = 8;
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
      int colors1 = gfx.Color.Get(-1, 100, 500, 555);
      int colors2 = gfx.Color.Get(-1, 100, 500, 532);
      if (this.Health < 200)
      {
        if (this.TickTime / 3 % 2 == 0)
        {
          colors1 = gfx.Color.Get(-1, 500, 100, 555);
          colors2 = gfx.Color.Get(-1, 500, 100, 532);
        }
      }
      else if (this.Health < 1000 && this.TickTime / 5 % 4 == 0)
      {
        colors1 = gfx.Color.Get(-1, 500, 100, 555);
        colors2 = gfx.Color.Get(-1, 500, 100, 532);
      }
      if (this.HurtTime > 0)
      {
        colors1 = gfx.Color.Get(-1, 555, 555, 555);
        colors2 = gfx.Color.Get(-1, 555, 555, 555);
      }
      screen.Render(num3 + 8 * bits1, yp, num1 + num2 * 32, colors1, bits1);
      screen.Render(num3 + 8 - 8 * bits1, yp, num1 + 1 + num2 * 32, colors1, bits1);
      screen.Render(num3 + 8 * bits2, yp + 8, num1 + (num2 + 1) * 32, colors2, bits2);
      screen.Render(num3 + 8 - 8 * bits2, yp + 8, num1 + 1 + (num2 + 1) * 32, colors2, bits2);
    }

    public override void TouchedBy(Entity entity)
    {
      if (!(entity is Player))
        return;

      entity.Hurt((Mob) this, 3, this.Dir);
    }

    protected override void DoHurt(int damage, int attackDir)
    {
      base.DoHurt(damage, attackDir);
      if (this.attackDelay != 0 || this.attackTime != 0)
        return;
      this.attackDelay = 120;
    }

    protected override void Die()
    {
      base.Die();
      if (this.MyLevel.MyPlayer != null)
      {
        this.MyLevel.MyPlayer.Score += 1000;
        this.MyLevel.MyPlayer.GameWon();
      }
      Sound.Bossdeath.Play();
    }
  }
}
