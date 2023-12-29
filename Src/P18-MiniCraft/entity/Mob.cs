// Decompiled with JetBrains decompiler
// Type: GameManager.entity.Mob

using GameManager.entity.particle;
using GameManager.gfx;
using GameManager.level;
using GameManager.level.tile;
using GameManager.sound;
using System.IO;

#nullable disable
namespace GameManager.entity
{
  public class Mob : Entity
  {
    public int Dir;
    public int HurtTime;
    public int MaxHealth = 10;
    public int Health = 10;
    public int SwimTimer;
    public int TickTime;
    protected int walkDist;
    protected int xKnockback;
    protected int yKnockback;

    public Mob()
    {
      this.X = this.Y = 8;
      this.Xr = 4;
      this.Yr = 3;
    }

    public Mob(Game1 game, BinaryReader reader)
      : base(game, reader)
    {
    }

    public override void SaveToWriter(Game1 game, BinaryWriter writer)
    {
      base.SaveToWriter(game, writer);
      writer.Write(this.walkDist);
      writer.Write(this.Dir);
      writer.Write(this.HurtTime);
      writer.Write(this.xKnockback);
      writer.Write(this.yKnockback);
      writer.Write(this.MaxHealth);
      writer.Write(this.Health);
      writer.Write(this.SwimTimer);
      writer.Write(this.TickTime);
    }

    public override void LoadFromReader(Game1 game, BinaryReader reader)
    {
      base.LoadFromReader(game, reader);
      this.walkDist = reader.ReadInt32();
      this.Dir = reader.ReadInt32();
      this.HurtTime = reader.ReadInt32();
      this.xKnockback = reader.ReadInt32();
      this.yKnockback = reader.ReadInt32();
      this.MaxHealth = reader.ReadInt32();
      this.Health = reader.ReadInt32();
      this.SwimTimer = reader.ReadInt32();
      this.TickTime = reader.ReadInt32();
    }

    public override void Tick()
    {
      ++this.TickTime;
      if (this.MyLevel.GetTile(this.X >> 4, this.Y >> 4) == Tile.Lava)
        this.Hurt(this, 4, this.Dir ^ 1);
      if (this.Health <= 0)
        this.Die();
      if (this.HurtTime <= 0)
        return;
      --this.HurtTime;
    }

    public override bool Move(int xa, int ya)
    {
      if (this.IsSwimming() && this.SwimTimer++ % 2 == 0)
        return true;
      if (this.xKnockback < 0)
      {
        this.Move2(-1, 0);
        ++this.xKnockback;
      }
      if (this.xKnockback > 0)
      {
        this.Move2(1, 0);
        --this.xKnockback;
      }
      if (this.yKnockback < 0)
      {
        this.Move2(0, -1);
        ++this.yKnockback;
      }
      if (this.yKnockback > 0)
      {
        this.Move2(0, 1);
        --this.yKnockback;
      }
      if (this.HurtTime > 0)
        return true;
      if (xa != 0 || ya != 0)
      {
        ++this.walkDist;
        if (xa < 0)
          this.Dir = 2;
        if (xa > 0)
          this.Dir = 3;
        if (ya < 0)
          this.Dir = 1;
        if (ya > 0)
          this.Dir = 0;
      }
      return base.Move(xa, ya);
    }

    public override bool Blocks(Entity e) => e.IsBlockableBy(this);

    public override void Hurt(Tile tile, int x, int y, int damage)
    {
      int attackDir = this.Dir ^ 1;
      this.DoHurt(damage, attackDir);
    }

        public override void Hurt(Mob mob, int damage, int attackDir)
        {
            this.DoHurt(damage, attackDir);
        }

        public void Heal(int heal)
    {
      if (this.HurtTime > 0)
        return;
      this.MyLevel.Add((Entity) new TextParticle(string.Empty + (object) heal, this.X, this.Y, 
          gfx.Color.Get(-1, 50, 50, 50)));
      this.Health += heal;
      if (this.Health <= this.MaxHealth)
        return;
      this.Health = this.MaxHealth;
    }

    public virtual bool FindStartPos(Level level)
    {
      int x = this.random.Next(level.W);
      int y = this.random.Next(level.H);
      int num1 = x * 16 + 8;
      int num2 = y * 16 + 8;
      if (level.MyPlayer != null)
      {
        int num3 = level.MyPlayer.X - num1;
        int num4 = level.MyPlayer.Y - num2;
        if (miniprefs.ScreenSize == 1)
        {
          if (num3 * num3 + num4 * num4 < 19200)
            return false;
        }
        else if (num3 * num3 + num4 * num4 < 6400)
          return false;
      }
      int num5 = level.MonsterDensity * 16;

      if (level.GetEntities(num1 - num5, num2 - num5, num1 + num5, num2 + num5).Count > 0 
                || !level.GetTile(x, y).MayPass(level, x, y, (Entity) this))
        return false;

      this.X = num1;
      this.Y = num2;
      return true;
    }

    protected virtual void DoHurt(int damage, int attackDir)
    {
      if (this.HurtTime > 0)
        return;

      if (this.MyLevel.MyPlayer != null)
      {
        int num1 = this.MyLevel.MyPlayer.X - this.X;
        int num2 = this.MyLevel.MyPlayer.Y - this.Y;
        if (miniprefs.ScreenSize == 1)
        {
          if (num1 * num1 + num2 * num2 < 25600)
            Sound.MonsterHurt.Play();
        }
        else if (num1 * num1 + num2 * num2 < 6400)
          Sound.MonsterHurt.Play();
      }
      this.MyLevel.Add((Entity) new TextParticle(string.Empty + (object) damage, this.X, this.Y, 
          gfx.Color.Get(-1, 500, 500, 500)));
      this.Health -= damage;
      if (attackDir == 0)
        this.yKnockback = 6;
      if (attackDir == 1)
        this.yKnockback = -6;
      if (attackDir == 2)
        this.xKnockback = -6;
      if (attackDir == 3)
        this.xKnockback = 6;
      this.HurtTime = 10;
    }

    protected bool IsSwimming()
    {
      Tile tile = this.MyLevel.GetTile(this.X >> 4, this.Y >> 4);
      return tile == Tile.Water || tile == Tile.Lava;
    }

    protected virtual void Die() => this.Remove();
  }
}
